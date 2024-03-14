namespace MyProClip.Services
{
    public class VideoService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VideoService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveVideoAsync(IFormFile videoFile)
        {
            ValidateVideo(videoFile);
            return await GenerateVideoNameAsync(videoFile);
        }

        private void ValidateVideo(IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                throw new Exception("No file provided or file is empty.");
            }

            string extension = Path.GetExtension(videoFile.FileName).ToLower();
            if (extension != ".mp4" && extension != ".webm")
            {
                throw new Exception("Invalid file format. Only MP4 and WebM files are allowed.");
            }
        }

        private async Task<string> GenerateVideoNameAsync(IFormFile videoFile)
        {
            Guid myuuid = Guid.NewGuid();

            string videoName = new String(Path.GetFileNameWithoutExtension(videoFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            videoName = videoName + DateTime.Now.ToString("yymmssfff") + myuuid.ToString() + Path.GetExtension(videoFile.FileName);
            var videoPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Videos", videoName);

            using (var fileStream = new FileStream(videoPath, FileMode.Create))
            {
                await videoFile.CopyToAsync(fileStream);
            }

            return videoName;
        }
    }
}
