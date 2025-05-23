﻿namespace testImageCompression
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.IO.Compression;

    class Program
    {
        static void Main()
        {
            string sourcePath = "C:\\Users\\HIBL\\Desktop\\New folder\\test.png";  // Replace with your image path
            //string destinationPath = "C:\\Users\\HIBL\\Desktop\\New folder\\output.png";
            //int quality = 50; // Compression quality (1-100)

            //CompressImage(sourcePath, destinationPath, quality);
            //Console.WriteLine("Image compression completed.");
            int orientation = GetImageOrientation(sourcePath);
            orientation=6;

            using (Bitmap bmp = new Bitmap(sourcePath))
            {
                var bmpR = RotateImage(bmp, orientation);
                bmpR.Save("C:\\Users\\HIBL\\Desktop\\New folder\\output.png", ImageFormat.Png);
            }

        }

        static void CompressImage(string sourcePath, string destinationPath, int quality)
        {
            using (Bitmap bmp = new Bitmap(sourcePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                bmp.Save(destinationPath, jpgEncoder, encoderParams);
            }
        }

        static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


       public static void ZipFiles(string sourceFolder, string zipPath)
        {
            if (Directory.Exists(sourceFolder))
            {
                ZipFile.CreateFromDirectory(sourceFolder, zipPath, CompressionLevel.Optimal, false);
            }
            else
            {
                Console.WriteLine("Source folder does not exist.");
            }
        }
        public static void UnzipFiles(string zipPath, string destinationFolder)
        {
            if (File.Exists(zipPath))
            {
                ZipFile.ExtractToDirectory(zipPath, destinationFolder);
            }
            else
            {
                Console.WriteLine("Zip file does not exist.");
            }
        }

        public static void SplitFile(string filePath, int chunkSize)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[chunkSize];
                int bytesRead;
                int partNumber = 0;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string partFileName = $"{filePath}.part{partNumber}";
                    using (FileStream partStream = new FileStream(partFileName, FileMode.Create, FileAccess.Write))
                    {
                        partStream.Write(buffer, 0, bytesRead);
                    }
                    partNumber++;
                }
            }
        }
        public static void MergeFiles(string[] partFiles, string outputFilePath)
        {
            using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                foreach (string partFile in partFiles)
                {
                    using (FileStream partStream = new FileStream(partFile, FileMode.Open, FileAccess.Read))
                    {
                        partStream.CopyTo(outputStream);
                    }
                }
            }
        }

        public static int GetImageOrientation(string imagePath)
        {
            using (Image img = Image.FromFile(imagePath))
            {
                foreach (PropertyItem prop in img.PropertyItems)
                {
                    if (prop.Id == 0x0112) // Orientation tag
                    {
                        return BitConverter.ToUInt16(prop.Value, 0);
                    }
                }
            }
            return 1; // Default orientation
        }
        public static Bitmap RotateImage(Bitmap img, int orientation)
        {
            switch (orientation)
            {
                case 3: img.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                case 6: img.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                case 8: img.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
            }
            return img;
        }

    }
}
