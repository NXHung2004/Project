namespace Project.Service
{
    public class SaveFileService
    {
        private readonly string _imageDirectory;

        public SaveFileService(IConfiguration configuration)
        {
            _imageDirectory = configuration["DirectoryPath:Path"];
        }

        public void CreateFile(byte[] imageData, string fileName)
        {
            string filePath = Path.Combine(_imageDirectory, fileName);
            File.WriteAllBytes(filePath, imageData);
        }
        public bool DeleteFile(string fileName)
        {
            string filePath = Path.Combine(_imageDirectory, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
    }
}
