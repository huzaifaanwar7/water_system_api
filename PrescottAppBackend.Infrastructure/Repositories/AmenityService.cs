using Microsoft.EntityFrameworkCore;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Infrastructure
{
    public class AmenityService : IAmenityService
    {
        private readonly PrescottContext _dbContext;

        public AmenityService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<AmenityVM>> GetAllAmenitiesAsync()
        {
            var list = await (from a in _dbContext.Amenities
                              join b in _dbContext.Buildings on a.BuildingId equals b.Id
                              join u in _dbContext.Users on a.CreatedBy equals u.Id
                              select new AmenityVM()
                              {
                                  Id = a.Id,
                                  BuildingId = a.BuildingId,
                                  AmenityName = a.AmenityName,
                                  Description = a.Description,
                                  CreatedBy = a.CreatedBy,
                                  CreatedAt = a.CreatedAt,
                                  UpdatedBy = a.UpdatedBy,
                                  UpdatedAt = a.UpdatedAt,
                                  BuildingName = b.BuildingName,
                                  CreatedByStr = (u.FirstName + ' ' + u.LastName).ToString(),
                                  UserVM = u
                              }).ToListAsync();
            if (list.Count > 0)
            {
                var amenityImages = await _dbContext.AmenityImages.Where(x => list.Select(a => a.Id).Contains(x.AmenityId)).ToListAsync();
                foreach (var r in list)
                {
                    var images = amenityImages.Where(x => x.AmenityId == r.Id).ToList();
                    r.AmenityImages = CustomMapper.MapList<AmenityImage, AmenityImageVM>(images);
                }
            }
            return list;
        }
        public async Task<AmenityVM> GetAmenityByIdAsync(int amenityId)
        {
            var result = await (from a in _dbContext.Amenities
                                join b in _dbContext.Buildings on a.BuildingId equals b.Id
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                where a.Id == amenityId
                                select new AmenityVM()
                                {
                                    Id = a.Id,
                                    BuildingId = a.BuildingId,
                                    AmenityName = a.AmenityName,
                                    Description = a.Description,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    BuildingName = b.BuildingName,
                                    CreatedByStr = (u.FirstName + ' ' + u.LastName).ToString(),
                                    UserVM = u
                                }).FirstOrDefaultAsync();


            var amenityImages = await _dbContext.AmenityImages.Where(x => x.AmenityId == result.Id).ToListAsync();
            result.AmenityImages = CustomMapper.MapList<AmenityImage, AmenityImageVM>(amenityImages);

            return result;
        }
        public async Task<AmenityVM> AddUpdateAmenityAsync(AmenityVM amenityVM)
        {
            if (amenityVM.Id == 0)
            {
                var amenity = new Amenity()
                {
                    BuildingId = amenityVM.BuildingId,
                    AmenityName = amenityVM.AmenityName,
                    Description = amenityVM.Description,
                    CreatedBy = amenityVM.CreatedBy,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                await _dbContext.Amenities.AddAsync(amenity);
                await _dbContext.SaveChangesAsync();

                var vM = CustomMapper.Map<Amenity, AmenityVM>(amenity);

                if (amenityVM.AmenityImages != null && amenityVM.AmenityImages.Count > 0)
                {
                    foreach (var images in amenityVM.AmenityImages)
                    {
                        string filePath = IOHelper.SaveFile(images.File, images.FileName);
                        var amenityImage = new AmenityImage()
                        {
                            FilePath = filePath,
                            FileName = images.FileName,
                            FileType = images.FileType,
                            AmenityId = amenity.Id,
                        };
                        await _dbContext.AmenityImages.AddAsync(amenityImage);
                        await _dbContext.SaveChangesAsync();

                        var imageVM = CustomMapper.Map<AmenityImage, AmenityImageVM>(amenityImage);
                        vM.AmenityImages.Add(imageVM);
                    }
                }


                return vM;
            }
            else
            {

                var amenity = await _dbContext.Amenities.FirstOrDefaultAsync(a => a.Id == amenityVM.Id);
                if (amenity != null)
                {

                    amenity.BuildingId = amenityVM.BuildingId;
                    amenity.AmenityName = amenityVM.AmenityName;
                    amenity.Description = amenityVM.Description;
                    amenity.UpdatedBy = amenityVM.UpdatedBy;
                    amenity.UpdatedAt = DateTime.Now;
                    amenity.IsDeleted = false;

                    var imagesToDelete = await _dbContext.AmenityImages.Where(a => a.AmenityId == amenity.Id && !amenityVM.AmenityImages.Select(img => img.Id).Contains(a.Id)).ToListAsync();

                    if (imagesToDelete.Any())
                    {
                        _dbContext.AmenityImages.RemoveRange(imagesToDelete);
                        await _dbContext.SaveChangesAsync();
                    }

                    var vM = CustomMapper.Map<Amenity, AmenityVM>(amenity);

                    if (amenityVM.AmenityImages != null && amenityVM.AmenityImages.Count > 0)
                    {
                        foreach (var images in amenityVM.AmenityImages)
                        {
                            string filePath = IOHelper.SaveFile(images.File, images.FileName);
                            var amenityImage = new AmenityImage()
                            {
                                FilePath = filePath,
                                FileName = images.FileName,
                                FileType = images.FileType,
                                AmenityId = amenity.Id,
                            };
                            await _dbContext.AmenityImages.AddAsync(amenityImage);
                            await _dbContext.SaveChangesAsync();

                            var imageVM = CustomMapper.Map<AmenityImage, AmenityImageVM>(amenityImage);
                            vM.AmenityImages.Add(imageVM);
                        }
                    }
                    _dbContext.Amenities.Update(amenity);
                    await _dbContext.SaveChangesAsync();

                    return vM;
                }

                return new AmenityVM();
            }
        }
        public async Task DeleteAmenityAsync(int amenityId)
        {
            var amenity = await _dbContext.Amenities.FindAsync(amenityId);
            if (amenity != null)
            {
                _dbContext.Amenities.Remove(amenity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<AmenityVM> GetAmenityByBuildingId(int buildingId)
        {
            var result = await (from a in _dbContext.Amenities
                                join b in _dbContext.Buildings on a.BuildingId equals b.Id
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                where a.BuildingId == buildingId
                                select new AmenityVM()
                                {
                                    Id = a.Id,
                                    BuildingId = a.BuildingId,
                                    AmenityName = a.AmenityName,
                                    Description = a.Description,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    BuildingName = b.BuildingName,
                                    CreatedByStr = (u.FirstName + ' ' + u.LastName).ToString(),
                                    UserVM = u
                                }).FirstOrDefaultAsync();

            if (result == null)
            {
                // Handle the null case (you can return null or throw a custom exception)
                throw new Exception("No amenities found for the given building ID.");
            }


            var amenityImages = await _dbContext.AmenityImages.Where(x => x.AmenityId == result.Id).ToListAsync();
            result.AmenityImages = CustomMapper.MapList<AmenityImage, AmenityImageVM>(amenityImages);

            return result;
        }

    }
}