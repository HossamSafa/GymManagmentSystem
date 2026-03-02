using GymManagmentBLL.Service.Interfaces.AttachmentService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes.AttachmentServices
{
	public class AttachmentSeervice : IAttachmentService
	{
		private readonly string[] allowedextention = { ".jpg", ".jpeg", ".png" };
		private readonly long MaxFileSize = 5 * 1025 * 1024; //5MB
		private readonly IWebHostEnvironment _webHost;

		public AttachmentSeervice(IWebHostEnvironment webHost)
		{
			_webHost = webHost;
		}

		public string? Upload(string folderName, IFormFile file)
		{
			try
			{
				if (folderName is null || file is null || file.Length == 0) return null;
				if (file.Length > MaxFileSize) return null;
				var extention = Path.GetExtension(file.FileName).ToLower();
				if (!allowedextention.Contains(extention)) return null;

				var folderpath = Path.Combine(_webHost.WebRootPath, "images", folderName);
				if (!Directory.Exists(folderpath))
				{
					Directory.CreateDirectory(folderpath);

				}
				var fileName = Guid.NewGuid().ToString() + extention;
				//wwwroot/images/members/5gdghgh5f4gfhjfjfh.png
				var filePath = Path.Combine(folderpath, fileName);
				using var fileStream = new FileStream(filePath, FileMode.Create);
				file.CopyTo(fileStream);
				return fileName;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Faild to upload file to folder ={folderName}:{ex}");
				return null;
			
			}
		}
		public bool Delete(string fileName, string folderName)
		{
			try
			{
				if(string.IsNullOrEmpty(fileName)||string.IsNullOrEmpty(folderName)) return false;

				var Fullpath=Path.Combine(_webHost.WebRootPath,"images",folderName,fileName);
				if(File.Exists(Fullpath))
				{
					File.Delete(Fullpath);
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Faild to delete file with name{fileName}:{ex}");
				return false;
			}
		}

	
	}
}
