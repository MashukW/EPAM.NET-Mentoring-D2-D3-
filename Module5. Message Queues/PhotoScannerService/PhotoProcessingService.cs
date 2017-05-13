using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Logging;

namespace PhotoScannerService
{
    public sealed class PhotoProcessingService : ServiceControl
    {
        private readonly FileSystemWatcher _watcher;
        private readonly Task _workTask;
        private readonly CancellationTokenSource _tokenSource;
        private readonly AutoResetEvent _newFileEvent;
        private readonly LogWriter _logWriter;

        private FileSystem FileSystem { get; }

        private int currentImageIndex;
        private int imageCount;
        private bool nextPageWaiting;
        
        public PhotoProcessingService(string inDirectory, string outDirectory, string tempDirectory)
        {
            _logWriter = HostLogger.Get<PhotoProcessingService>();

            FileSystem = new FileSystem(inDirectory, outDirectory, tempDirectory);

            _watcher = new FileSystemWatcher();
            _watcher.Created += (sender, args) => _newFileEvent.Set();
            _tokenSource = new CancellationTokenSource();
            _workTask = new Task(() => WorkProcedure(_tokenSource.Token));
            _newFileEvent = new AutoResetEvent(false);

            InitializeStartValues();
        }

        public bool Start(HostControl hostControl)
        {
            _workTask.Start();
            _watcher.EnableRaisingEvents = true;
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _watcher.EnableRaisingEvents = false;
            _tokenSource.Cancel();
            _workTask.Wait();
            return true;
        }

        private static bool IsValidFormat(string fileName) => Regex.IsMatch(fileName, @"^image_[0-9]{1,3}.(jpg|png|jpeg)$");

        private static int GetIndex(string fileName)
        {
            var match = Regex.Match(fileName, @"[0-9]{3}");

            return match.Success ? int.Parse(match.Value) : -1;
        }

        public void WorkProcedure(CancellationToken token)
        {
            var pdf = PdfDocument.CreateNewDocument();

            do
            {
                foreach (var file in FileSystem.InputDirectory.GetFiles().Skip(imageCount))
                {
                    var fileName = file.Name;
                    if (IsValidFormat(fileName))
                    {
                        var imageIndex = GetIndex(fileName);
                        if (imageIndex != currentImageIndex + 1 && currentImageIndex != -1 && nextPageWaiting)
                        {
                            SavePdf(ref pdf);
                        }

                        if (FileSystem.TryOpenFile(file, 3))
                        {
                            pdf.AddImage(file.FullName);
                            imageCount++;
                            currentImageIndex = imageIndex;
                            nextPageWaiting = true;
                        }
                    }
                    else
                    {
                        var outFile = Path.Combine(FileSystem.TempDirectory.FullName, fileName);
                        if (File.Exists(outFile))
                        {
                            FileSystem.DeleteFile(file);
                        }
                        else
                        {
                            file.MoveTo(outFile);
                        }
                    }
                }

                if (!_newFileEvent.WaitOne(5000) && nextPageWaiting)
                {
                    SavePdf(ref pdf);
                }

                if (token.IsCancellationRequested)
                {
                    if (nextPageWaiting)
                    {
                        pdf.Save(FileSystem.GetNextFilename());
                    }

                    FileSystem.ClearInputDirectory();
                }
            } while (!token.IsCancellationRequested);
        }

        private void SavePdf(ref PdfDocument pdf)
        {
            pdf.Save(FileSystem.GetNextFilename());
            pdf = PdfDocument.CreateNewDocument();
            
            FileSystem.ClearInputDirectory(imageCount);
            InitializeStartValues();
        }

        private void InitializeStartValues()
        {
            currentImageIndex = -1;
            imageCount = 0;
            nextPageWaiting = false;
        }
    }
}
