using Microsoft.EntityFrameworkCore;
using GlobularsAdmin.Domain;
using GlobularsAdmin.Domain.DbModels;
using System.Collections.Generic;

namespace GlobularsAdmin.Infrastructure
{
    public class ReportedProblemService : IReportedProblemService
    {
        private readonly PrescottContext _dbContext;

        public ReportedProblemService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<List<ReportedProblemVM>> GetAllReportedProblemsAsync()
        {
            var list = await (from a in _dbContext.ReportedProblems
                              join u in _dbContext.Users on a.CreatedBy equals u.Id
                              select new ReportedProblemVM()
                              {
                                  Id = a.Id,
                                  Title = a.Title,
                                  Problem = a.Problem,
                                  CreatedBy = a.CreatedBy,
                                  CreatedAt = a.CreatedAt,
                                  UpdatedBy = a.UpdatedBy,
                                  UpdatedAt = a.UpdatedAt,
                                  CreatedByStr = (u.FirstName + ' ' + u.LastName).ToString(),
                                  UserVM = u
                              }).ToListAsync();
            if (list.Count > 0)
            {
                var problemImages = await _dbContext.ReportedProblemImages.Where(x => list.Select(a => a.Id).Contains(x.ReportedProblemId)).ToListAsync();
                foreach (var r in list)
                {
                    var images = problemImages.Where(x => x.ReportedProblemId == r.Id).ToList();
                    r.ReportedProblemImages = CustomMapper.MapList<ReportedProblemImage, ReportedProblemImageVM>(images);
                }
            }
            return list;
        }
        public async Task<ReportedProblemVM> GetReportedProblemByIdAsync(int problemId)
        {
            var result = await (from a in _dbContext.ReportedProblems
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                where a.Id == problemId
                                select new ReportedProblemVM()
                                {
                                    Id = a.Id,
                                    Title = a.Title,
                                    Problem = a.Problem,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    CreatedByStr = (u.FirstName + ' ' + u.LastName).ToString(),
                                    UserVM = u
                                }).FirstOrDefaultAsync();


            if (result != null && result.Id > 0)
            {
                var problemImages = await _dbContext.ReportedProblemImages.Where(x => x.ReportedProblemId == result.Id).ToListAsync();
                result.ReportedProblemImages = CustomMapper.MapList<ReportedProblemImage, ReportedProblemImageVM>(problemImages);
            }

            return result;
        }
        public async Task<ReportedProblemVM> AddUpdateReportedProblemAsync(ReportedProblemVM problemVM)
        {
            if (problemVM.Id == 0)
            {
                var problem = new ReportedProblem()
                {
                    Title = problemVM.Title,
                    Problem = problemVM.Problem,
                    CreatedBy = problemVM.CreatedBy,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                await _dbContext.ReportedProblems.AddAsync(problem);
                await _dbContext.SaveChangesAsync();

                var vM = CustomMapper.Map<ReportedProblem, ReportedProblemVM>(problem);

                if (problemVM.ReportedProblemImages != null && problemVM.ReportedProblemImages.Count > 0)
                {
                    foreach (var images in problemVM.ReportedProblemImages)
                    {
                        string filePath = IOHelper.SaveFile(images.File, images.FileName);
                        var problemImage = new ReportedProblemImage()
                        {
                            FilePath = filePath,
                            FileName = images.FileName,
                            FileType = images.FileType,
                            ReportedProblemId = problem.Id,
                        };
                        await _dbContext.ReportedProblemImages.AddAsync(problemImage);
                        await _dbContext.SaveChangesAsync();

                        var imageVM = CustomMapper.Map<ReportedProblemImage, ReportedProblemImageVM>(problemImage);
                        vM.ReportedProblemImages.Add(imageVM);
                    }
                }


                return vM;
            }
            else
            {

                var problem = await _dbContext.ReportedProblems.FirstOrDefaultAsync(a => a.Id == problemVM.Id);
                if (problem != null)
                {
                    problem.Title = problemVM.Title;
                    problem.Problem = problemVM.Problem;
                    problem.UpdatedBy = problemVM.UpdatedBy;
                    problem.UpdatedAt = DateTime.Now;
                    problem.IsDeleted = false;

                    var imagesToDelete = await _dbContext.ReportedProblemImages.Where(a => a.ReportedProblemId == problem.Id && !problemVM.ReportedProblemImages.Select(img => img.Id).Contains(a.Id)).ToListAsync();

                    if (imagesToDelete.Any())
                    {
                        _dbContext.ReportedProblemImages.RemoveRange(imagesToDelete);
                        await _dbContext.SaveChangesAsync();
                    }

                    var vM = CustomMapper.Map<ReportedProblem, ReportedProblemVM>(problem);

                    if (problemVM.ReportedProblemImages != null && problemVM.ReportedProblemImages.Count > 0)
                    {
                        foreach (var images in problemVM.ReportedProblemImages)
                        {
                            string filePath = IOHelper.SaveFile(images.File, images.FileName);
                            var problemImage = new ReportedProblemImage()
                            {
                                FilePath = filePath,
                                FileName = images.FileName,
                                FileType = images.FileType,
                                ReportedProblemId = problem.Id,
                            };
                            await _dbContext.ReportedProblemImages.AddAsync(problemImage);
                            await _dbContext.SaveChangesAsync();

                            var imageVM = CustomMapper.Map<ReportedProblemImage, ReportedProblemImageVM>(problemImage);
                            vM.ReportedProblemImages.Add(imageVM);
                        }
                    }
                    _dbContext.ReportedProblems.Update(problem);
                    await _dbContext.SaveChangesAsync();

                    return vM;
                }

                return new ReportedProblemVM();
            }
        }
        public async Task DeleteReportedProblemAsync(int problemId)
        {
            var problem = await _dbContext.ReportedProblems.FindAsync(problemId);
            if (problem != null)
            {
                _dbContext.ReportedProblems.Remove(problem);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ReportedProblemVM>> GetReportedProblemByUser(string userId)
        {
            var result = await (from a in _dbContext.ReportedProblems
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                where a.CreatedBy == userId
                                select new ReportedProblemVM()
                                {
                                    Id = a.Id,
                                    Title = a.Title,
                                    Problem = a.Problem,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    CreatedByStr = (u.FirstName + ' ' + u.LastName).ToString(),
                                    UserVM = u
                                }).ToListAsync();

            if (result == null)
            {
                // Handle the null case (you can return null or throw a custom exception)
                throw new Exception("No reports found for the given user ID.");
            }

            if (result.Count > 0)
            {
                var problemImages = await _dbContext.ReportedProblemImages.Where(x => result.Select(a => a.Id).Contains(x.ReportedProblemId)).ToListAsync();
                foreach (var r in result)
                {
                    var images = problemImages.Where(x => x.ReportedProblemId == r.Id).ToList();
                    r.ReportedProblemImages = CustomMapper.MapList<ReportedProblemImage, ReportedProblemImageVM>(images);
                }
            }
            return result;
        }

    }
}