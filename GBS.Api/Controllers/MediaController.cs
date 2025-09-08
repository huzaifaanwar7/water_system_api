using Azure;
using GBS.Api.Model;
using GBS.Data.Model;
using GBS.Entities.DbModels;
using GBS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;

namespace GBS.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MediaController(IOptions<AppSettings> appSettings, GBS_DbContext _dbContext, IMediaService _mediaService) : ControllerBase
    {
        private readonly AppSettings _appSettings = appSettings.Value;

        [HttpPost("Upload")]
        //[Consumes("multipart/form-data")]
        //[Produces("application/json")]

        public async Task<IActionResult> Upload(
            [FromHeader(Name = "X-Application-Bucket")] string bucketName,
            [FromHeader(Name = "X-Application")] string application)
        {
            try
            {
                if (string.IsNullOrEmpty(application))
                {
                    throw new Exception("Missing X-Application in the Header");
                }

                if (string.IsNullOrEmpty(bucketName))
                {
                    throw new Exception("Missing X-Application-Bucket in the Header");
                }

                if (Request.Form.Files.Count <= 0)
                {
                    throw new Exception("Missing Media File in the Body");
                }

                var mediaFormFile = Request.Form.Files[0];
                var newId = Guid.NewGuid();

                string uploadPath = Path.Combine(_appSettings.MediaPath, $"{application}/{bucketName}");

                var contentType = mediaFormFile.ContentType;
                var extension = Path.GetExtension(mediaFormFile.FileName);

                //string _allowedExtensions = _appSettings.AllowedFileExtensions;

                //if (!_allowedExtensions.Any(extension.Contains))
                //{
                //    throw new BadRequestException("File type is not allowed");
                //}

                // Ensure the directory exists; create it if necessary
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                string uniqueFileName = $"{newId}-{mediaFormFile.FileName}";
                string filePath = Path.Combine(uploadPath, uniqueFileName);
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    mediaFormFile.CopyTo(stream);
                //}

                using (var memoryStream = new MemoryStream())
                {
                    // Copy the content of the IFormFile into the MemoryStream
                    mediaFormFile.CopyTo(memoryStream);

                    // Convert the MemoryStream to a byte array
                    byte[] fileBytes = memoryStream.ToArray();

                    // Combine the directory path and the file name to get the full file path
                    string fPath = Path.Combine(uploadPath, uniqueFileName);

                    // Write the byte array to the file
                    System.IO.File.WriteAllBytes(fPath, fileBytes);
                }

                var MediaFile = new MediaFile
                {
                    Id = newId,
                    Name = uniqueFileName,
                    MimeType = mediaFormFile.ContentType,
                    Size = mediaFormFile.Length,
                    Bucket = bucketName,
                    CreatedBy = application,
                };
                var saveFile = await _mediaService.Save(MediaFile);


                if (saveFile > 0)
                {
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "File saved successfully",
                        data = MediaFile
                    });
                }
                else
                {
                    throw new Exception("No Entries Created. Failed to Save!");
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.InnerException}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> View(Guid id)
        {
            try
            {
                if (id == null) { return NotFound(); }
                var media = await _mediaService.Get(id);
                //var mediaDataSet = await sqlServerClient.ExecuteAsync("SELECT Id, Content, MimeType, Bucket, CreatedBy,Name FROM MEDIA WHERE Id = @Id", CommandType.Text, [new SqlParameter("@Id", id)]);

                //if (!mediaDataSet.HasData())
                //{
                //    return NotFound();
                //}

                //var media = mediaDataSet.Tables[0].AsEnumerable().Select(row => new Media
                //{
                //    Id = row.Field<Guid>("Id"),
                //    Content = row.Field<byte[]>("Content")!,
                //    Name = row.Field<string>("Name") ?? "",
                //    MimeType = row.Field<string>("MimeType") ?? "",
                //    CreatedBy = row.Field<string>("CreatedBy") ?? "",
                //    Bucket = row.Field<string>("Bucket") ?? ""
                //}).ToList()?.First();

                if (media == null)
                {
                    return NotFound();
                }

                string fileDirectory = Path.Combine(_appSettings.MediaPath, $"{media.CreatedBy}/{media.Bucket}");

                string filePath = Path.Combine(fileDirectory, media.Name);
                string absolutePath = Path.GetFullPath(filePath);
                if (System.IO.File.Exists(absolutePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    string contentType = media.MimeType;
                    if (string.IsNullOrEmpty(media.MimeType))
                    {
                        var provider = new FileExtensionContentTypeProvider();
                        if (provider.TryGetContentType(filePath, out contentType))
                        {
                            SqlParameter[] paramz = [
                                new SqlParameter("@Id", id),
                                new SqlParameter("@MimeType", contentType),
                                new SqlParameter("@Media", fileBytes),
                            ];
                            media.MimeType = contentType;
                            //int rowsAffected = _mediaService.Update(media);
                        }
                    }

                    return File(fileBytes, contentType, media.Name);
                }
                else
                {

                    //byte[] mediaContent;
                    //try
                    //{
                    //    mediaContent = ResizeIfRequired(media.Content, Request.Query);
                    //}
                    //catch (Exception)
                    //{
                    //    mediaContent = media.Content;
                    //}

                    //Response.Headers.Append("X-Content-Type", media.MimeType);

                    //return File(mediaContent, media.MimeType);
                    return null;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "Failure",
                    message = ex.Message
                });
            }
        }



        [HttpGet("{id}/download")]
        //
        public async Task<IActionResult> Download(Guid id)
        {
            return await View(id);
            //try
            //{
            //    var mediaDataSet = await sqlServerClient.ExecuteAsync("SELECT Id, Content, MimeType, Bucket, CreatedBy,Name FROM MEDIA WHERE Id = @Id", CommandType.Text, [new SqlParameter("@Id", id)]);

            //    if (!mediaDataSet.HasData())
            //    {
            //        return NotFound();
            //    }

            //    var media = mediaDataSet.Tables[0].AsEnumerable().Select(row => new Media
            //    {
            //        Id = row.Field<Guid>("Id"),
            //        Content = row.Field<byte[]>("Content")!,
            //        Name = row.Field<string>("Name") ?? "",
            //        MimeType = row.Field<string>("MimeType") ?? "",
            //        CreatedBy = row.Field<string>("CreatedBy") ?? "",
            //        Bucket = row.Field<string>("Bucket") ?? ""
            //    }).ToList()?.First();

            //    if (media == null)
            //    {
            //        return NotFound();
            //    }

            //    if (media.CreatedBy.Equals("RFDT", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        media.CreatedBy = "RFDT";
            //    }
            //    string fileDirectory = Path.Combine(_appSettings.MediaPath, $"{media.CreatedBy}/{media.Bucket}");
            //    string filePath = Path.Combine(fileDirectory, media.Name);

            //    if (System.IO.File.Exists(filePath))
            //    {
            //        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //        string contentType = media.MimeType; // Implement this method to determine the content type based on the file extension
            //        if (string.IsNullOrEmpty(media.MimeType))
            //        {
            //            var provider = new FileExtensionContentTypeProvider();
            //            provider.Mappings[".msg"] = "application/vnd.ms-outlook";
            //            if (provider.TryGetContentType(filePath, out contentType))
            //            {
            //                SqlParameter[] paramz = [
            //                    new SqlParameter("@Id", id),
            //                new SqlParameter("@MimeType", contentType),
            //                new SqlParameter("@Media", fileBytes),
            //            ];
            //                int rowsAffected = sqlServerClient.ExecuteNonQuery($"UPDATE MEDIA SET MimeType = @MimeType WHERE Id = @Id", CommandType.Text, paramz);
            //            }
            //            else
            //            {
            //                return StatusCode(StatusCodes.Status500InternalServerError, new
            //                {
            //                    status = "Failure",
            //                    message = "Failed to get content-type"
            //                });
            //            }
            //        }

            //        return File(fileBytes, contentType, media.Name);
            //    }
            //    else
            //    {
            //        var mediaContent = media.MimeType.ToLower().Contains("image") ?
            //            ResizeIfRequired(media.Content, Request.Query) :
            //            media.Content;

            //        Response.Headers.Append("Content-Disposition", $"attachment; filename={media.Name}");
            //        return File(mediaContent, media.MimeType);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new
            //    {
            //        status = "Failure",
            //        message = ex.Message.Contains("invalid values at index 0") ? "File does not exists in directory" : ex.Message
            //    });
            //}
        }

        private byte[] ResizeIfRequired(byte[] mediaContent, IQueryCollection query)
        {
            try
            {
                //if (query.ContainsKey("w") && query.ContainsKey("h"))
                //{
                //    var height = Request.Query["h"].FirstOrDefault() ?? "0";
                //    var width = Request.Query["w"].FirstOrDefault() ?? "0";
                //    return MediaUtil.Resize(mediaContent, int.Parse(width), int.Parse(height));
                //}
                return mediaContent;
            }
            catch (Exception ex) { return mediaContent; }
        }


    }
}
