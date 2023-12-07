using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OfficeOpenXml;
using Project.Dto;
using Project.Service;
using System.Security.Claims;

namespace Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaveFileController : ControllerBase
    {
        private readonly SaveFileService _saveFile;
        private readonly EmailService _emailService;
        public SaveFileController(SaveFileService saveFileService, EmailService emailService)
        {
            _saveFile = saveFileService;
            _emailService = emailService;
        }
        [HttpPost]
        public string Savefile([FromForm] IFormFile? upload)
        {
            if (upload == null)
            {
                throw new ArgumentNullException("Không nhận được file");
            }
            var fileName = upload.FileName;
            var contentType = upload.ContentType;
            using (var stream = new MemoryStream())
            {
                upload.CopyTo(stream);
                byte[] imageData = stream.ToArray();
                _saveFile.CreateFile(imageData, fileName);
            }
            return fileName;
        }
        [HttpDelete]
        public string Delete(string fileName)
        {
            var status = _saveFile.DeleteFile(fileName);
            if (status) return "Thành công";
            return "Thất bại";
        }
        [HttpPost("FileExcel")]
        public List<QuestionsDto> FileExcel([FromForm] IFormFile? upload)
        {
            var result = new List<QuestionsDto>();
            if (upload == null)
            {
                throw new ArgumentNullException("Không có file");
            }
            var fileName = upload.FileName;
            using (var package = new ExcelPackage(upload.OpenReadStream()))
            {
                // Lấy ra sheet đầu tiên trong Excel workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Lấy số dòng và số cột trong sheet
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                var question = new QuestionsDto();
                var answers = new List<Answer>();
                // Đọc dữ liệu từ sheet
                for (int row = 2; row <= rowCount; row++)
                {
                    var answer = new Answer();
                    for (int col = 1; col <= colCount; col++)
                    {
                        if(worksheet.Cells[row, 1].Value == null)
                        {
                            if(col == 2)
                            {
                                if(question.Name != null)
                                {
                                    question.Answers = answers;
                                    result.Add(question);
                                    question = new QuestionsDto();
                                    answers = new List<Answer>();
                                }
                                question.Name = worksheet.Cells[row, col].Value.ToString();
                            }
                        }
                        else
                        {
                            if(col == 1)
                            {
                                answer.Code = worksheet.Cells[row, col].Value.ToString();
                            }
                            else if(col == 2)
                            {
                                answer.Name = worksheet.Cells[row, col].Value.ToString();
                            }
                            else if( col == 3)
                            {
                                var stringStatus = worksheet.Cells[row, col].Value.ToString();
                                if (int.TryParse(stringStatus, out int Value))
                                {
                                    answer.Status = Value;
                                }
                                else
                                {
                                    throw new ArgumentNullException("status chỉ đc chứa 1 hoặc 0");
                                }
                            }
                        }
                    }
                    if(answer.Name != null)
                    {
                        answers.Add(answer);
                    }
                    if(row +1 == rowCount)
                    {
                        question.Answers = answers;
                        result.Add(question);
                    }
                }
            }
            return result;
        }
        [HttpGet]
        public async Task<string> SendEmail()
        {
            await _emailService.SendEMailAsync("hungvea@gmail.com", "test", "hungne");
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = userIdClaim.Value;
            return userId;
        }
    }
}
