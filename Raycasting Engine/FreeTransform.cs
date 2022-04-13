using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Raycasting_Engine
{
    public class FreeTransform
    {
        PointF[] vertex=new PointF[4];
        Vector AB, BC, CD, DA;
        Rectangle rect = new Rectangle();
        ImageData srcCB = new ImageData();
        int srcW = 0;
        int srcH = 0;

        public Bitmap Bitmap
        {
            set
            {
                try
                {
                    srcCB.FromBitmap(value);
                    srcH = value.Height;
                    srcW = value.Width;
                }
                catch
                {
                    srcW = 0; srcH = 0;
                }
            }
            get
            {
                return getTransformedBitmap();
            }
        }

        public Point ImageLocation
        {
            set { rect.Location = value; }
            get { return rect.Location; }
        }

        bool isBilinear = false;
        public bool IsBilinearInterpolation
        {
            set { isBilinear = value; }
            get { return isBilinear; }
        }

        public int ImageWidth
        {
            get { return rect.Width; }
        }

        public int ImageHeight
        {
            get { return rect.Height; }
        }

        public PointF VertexLeftTop
        {
            set { vertex[0] = value; setVertex(); }
            get { return vertex[0]; }
        }

        public PointF VertexTopRight
        {
            set { vertex[1] = value; setVertex(); }
            get { return vertex[1]; }
        }

        public PointF VertexRightBottom
        {
            set { vertex[2] = value; setVertex(); }
            get { return vertex[2]; }
        }

        public PointF VertexBottomLeft
        {
            set { vertex[3] = value; setVertex(); }
            get { return vertex[3]; }
        }

        public PointF[] FourCorners
        {
            set { vertex = value; setVertex(); }
            get { return vertex; }
        }

        private void setVertex()
        {
            float xmin = float.MaxValue;
            float ymin = float.MaxValue;
            float xmax = float.MinValue;
            float ymax = float.MinValue;

            for (int i = 0; i < 4; i++)
            {
                xmax = Math.Max(xmax, vertex[i].X);
                ymax = Math.Max(ymax, vertex[i].Y);
                xmin = Math.Min(xmin, vertex[i].X);
                ymin = Math.Min(ymin, vertex[i].Y);
            }

            rect = new Rectangle((int)xmin, (int)ymin, (int)(xmax - xmin), (int)(ymax - ymin));

            AB = new Vector(vertex[0], vertex[1]);
            BC = new Vector(vertex[1], vertex[2]);
            CD = new Vector(vertex[2], vertex[3]);
            DA = new Vector(vertex[3], vertex[0]);

            // get unit vector
            AB /= AB.Magnitude;
            BC /= BC.Magnitude;
            CD /= CD.Magnitude;
            DA /= DA.Magnitude;
        }

        private bool isOnPlaneABCD(PointF pt) //  including point on border
        {
            if (!Vector.IsCCW(pt, vertex[0], vertex[1]))
            {
                if (!Vector.IsCCW(pt, vertex[1], vertex[2]))
                {
                    if (!Vector.IsCCW(pt, vertex[2], vertex[3]))
                    {
                        if (!Vector.IsCCW(pt, vertex[3], vertex[0]))
                            return true;
                    }
                }
            }
            return false;
        }

        private Bitmap getTransformedBitmap()
        {
            if (srcH == 0 || srcW == 0) return null;

            ImageData destCB = new ImageData();
            destCB.A = new byte[rect.Width, rect.Height];
            destCB.B = new byte[rect.Width, rect.Height];
            destCB.G = new byte[rect.Width, rect.Height];
            destCB.R = new byte[rect.Width, rect.Height];

           
            PointF ptInPlane = new PointF();
            int x1, x2, y1, y2;
            double dab, dbc, dcd, dda;
            float dx1, dx2, dy1, dy2, dx1y1, dx1y2, dx2y1, dx2y2, nbyte;

            for (int y = 0; y < rect.Height; y++)
            {
                for (int x = 0; x < rect.Width; x++)
                {
                    Point srcPt = new Point(x, y);
                    srcPt.Offset(this.rect.Location);

                    if (isOnPlaneABCD(srcPt))
                    {
                        dab = Math.Abs((new Vector(vertex[0], srcPt)).CrossProduct(AB));
                        dbc = Math.Abs((new Vector(vertex[1], srcPt)).CrossProduct(BC));
                        dcd = Math.Abs((new Vector(vertex[2], srcPt)).CrossProduct(CD));
                        dda = Math.Abs((new Vector(vertex[3], srcPt)).CrossProduct(DA));
                        ptInPlane.X = (float)(srcW * (dda / (dda + dbc)));
                        ptInPlane.Y = (float)(srcH * (dab / (dab + dcd)));

                        x1 = (int)ptInPlane.X;
                        y1 = (int)ptInPlane.Y;

                        if (x1 >= 0 && x1 < srcW && y1 >= 0 && y1 < srcH)
                        {
                            if (isBilinear)
                            {
                                x2 = (x1 == srcW - 1) ? x1 : x1 + 1;
                                y2 = (y1 == srcH - 1) ? y1 : y1 + 1;

                                dx1 = ptInPlane.X - (float)x1;
                                if (dx1 < 0) dx1 = 0;
                                dx1 = 1f - dx1;
                                dx2 = 1f - dx1;
                                dy1 = ptInPlane.Y - (float)y1;
                                if (dy1 < 0) dy1 = 0;
                                dy1 = 1f - dy1;
                                dy2 = 1f - dy1;

                                dx1y1 = dx1 * dy1;
                                dx1y2 = dx1 * dy2;
                                dx2y1 = dx2 * dy1;
                                dx2y2 = dx2 * dy2;


                                nbyte = srcCB.A[x1, y1] * dx1y1 + srcCB.A[x2, y1] * dx2y1 + srcCB.A[x1, y2] * dx1y2 + srcCB.A[x2, y2] * dx2y2;
                                destCB.A[x, y] = (byte)nbyte;
                                nbyte = srcCB.B[x1, y1] * dx1y1 + srcCB.B[x2, y1] * dx2y1 + srcCB.B[x1, y2] * dx1y2 + srcCB.B[x2, y2] * dx2y2;
                                destCB.B[x, y] = (byte)nbyte;
                                nbyte = srcCB.G[x1, y1] * dx1y1 + srcCB.G[x2, y1] * dx2y1 + srcCB.G[x1, y2] * dx1y2 + srcCB.G[x2, y2] * dx2y2;
                                destCB.G[x, y] = (byte)nbyte;
                                nbyte = srcCB.R[x1, y1] * dx1y1 + srcCB.R[x2, y1] * dx2y1 + srcCB.R[x1, y2] * dx1y2 + srcCB.R[x2, y2] * dx2y2;
                                destCB.R[x, y] = (byte)nbyte;
                            }
                            else
                            {
                                destCB.A[x, y] = srcCB.A[x1, y1];
                                destCB.B[x, y] = srcCB.B[x1, y1];
                                destCB.G[x, y] = srcCB.G[x1, y1];
                                destCB.R[x, y] = srcCB.R[x1, y1];
                            }
                        }
                    }
                }
            }
            return destCB.ToBitmap();
        }
    }
    public struct Vector
    {
        double _x, _y;

        public Vector(double x, double y)
        {
            _x = x; _y = y;
        }
        public Vector(PointF pt)
        {
            _x = pt.X;
            _y = pt.Y;
        }
        public Vector(PointF st, PointF end)
        {
            _x = end.X - st.X;
            _y = end.Y - st.Y;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Magnitude
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.X, -v.Y);
        }

        public static Vector operator *(double c, Vector v)
        {
            return new Vector(c * v.X, c * v.Y);
        }

        public static Vector operator *(Vector v, double c)
        {
            return new Vector(c * v.X, c * v.Y);
        }

        public static Vector operator /(Vector v, double c)
        {
            return new Vector(v.X / c, v.Y / c);
        }

        // A * B =|A|.|B|.sin(angle AOB)
        public double CrossProduct(Vector v)
        {
            return _x * v.Y - v.X * _y;
        }

        // A. B=|A|.|B|.cos(angle AOB)
        public double DotProduct(Vector v)
        {
            return _x * v.X + _y * v.Y;
        }

        public static bool IsClockwise(PointF pt1, PointF pt2, PointF pt3)
        {
            Vector V21 = new Vector(pt2, pt1);
            Vector v23 = new Vector(pt2, pt3);
            return V21.CrossProduct(v23) < 0; // sin(angle pt1 pt2 pt3) > 0, 0<angle pt1 pt2 pt3 <180
        }

        public static bool IsCCW(PointF pt1, PointF pt2, PointF pt3)
        {
            Vector V21 = new Vector(pt2, pt1);
            Vector v23 = new Vector(pt2, pt3);
            return V21.CrossProduct(v23) > 0;  // sin(angle pt2 pt1 pt3) < 0, 180<angle pt2 pt1 pt3 <360
        }

        public static double DistancePointLine(PointF pt, PointF lnA, PointF lnB)
        {
            Vector v1 = new Vector(lnA, lnB);
            Vector v2 = new Vector(lnA, pt);
            v1 /= v1.Magnitude;
            return Math.Abs(v2.CrossProduct(v1));
        }

        public void Rotate(int Degree)
        {
            double radian = Degree * Math.PI / 180.0;
            double sin = Math.Sin(radian);
            double cos = Math.Cos(radian);
            double nx = _x * cos - _y * sin;
            double ny = _x * sin + _y * cos;
            _x = nx;
            _y = ny;
        }

        public PointF ToPointF()
        {
            return new PointF((float)_x, (float)_y);
        }
    }
    public class ImageData : IDisposable
    {
        private byte[,] _red, _green, _blue, _alpha;
        private bool _disposed = false;

        public byte[,] A
        {
            get { return _alpha; }
            set { _alpha = value; }
        }
        public byte[,] B
        {
            get { return _blue; }
            set { _blue = value; }
        }
        public byte[,] G
        {
            get { return _green; }
            set { _green = value; }
        }
        public byte[,] R
        {
            get { return _red; }
            set { _red = value; }
        }

        public ImageData Clone()
        {
            ImageData cb = new ImageData();
            cb.A = (byte[,])_alpha.Clone();
            cb.B = (byte[,])_blue.Clone();
            cb.G = (byte[,])_green.Clone();
            cb.R = (byte[,])_red.Clone();
            return cb;
        }

        # region InteropServices.Marshal mathods
        public void FromBitmap(Bitmap srcBmp)
        {
            int w = srcBmp.Width;
            int h = srcBmp.Height;

            _alpha = new byte[w, h];
            _blue = new byte[w, h];
            _green = new byte[w, h];
            _red = new byte[w, h];

            // Lock the bitmap's bits.  
            System.Drawing.Imaging.BitmapData bmpData = srcBmp.LockBits(new Rectangle(0, 0, w, h),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * srcBmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            int offset = bmpData.Stride - w * 4;

            int index = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    _blue[x, y] = rgbValues[index];
                    _green[x, y] = rgbValues[index + 1];
                    _red[x, y] = rgbValues[index + 2];
                    _alpha[x, y] = rgbValues[index + 3];
                    index += 4;
                }
                index += offset;
            }

            // Unlock the bits.
            srcBmp.UnlockBits(bmpData);
        }

        public Bitmap ToBitmap()
        {
            int width = 0, height = 0;
            if (_alpha != null)
            {
                width = Math.Max(width, _alpha.GetLength(0));
                height = Math.Max(height, _alpha.GetLength(1));
            }
            if (_blue != null)
            {
                width = Math.Max(width, _blue.GetLength(0));
                height = Math.Max(height, _blue.GetLength(1));
            }
            if (_green != null)
            {
                width = Math.Max(width, _green.GetLength(0));
                height = Math.Max(height, _green.GetLength(1));
            }
            if (_red != null)
            {
                width = Math.Max(width, _red.GetLength(0));
                height = Math.Max(height, _red.GetLength(1));
            }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // set rgbValues
            int offset = bmpData.Stride - width * 4;
            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rgbValues[i] = checkArray(_blue, x, y) ? _blue[x, y] : (byte)0;
                    rgbValues[i + 1] = checkArray(_green, x, y) ? _green[x, y] : (byte)0;
                    rgbValues[i + 2] = checkArray(_red, x, y) ? _red[x, y] : (byte)0;
                    rgbValues[i + 3] = checkArray(_alpha, x, y) ? _alpha[x, y] : (byte)255;
                    i += 4;
                }
                i += offset;
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        # endregion

        private static bool checkArray(byte[,] array, int x, int y)
        {
            if (array == null) return false;
            if (x < array.GetLength(0) && y < array.GetLength(1))
                return true;
            else return false;
        }

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    _alpha = null;
                    _blue = null;
                    _green = null;
                    _red = null;
                }

                // Indicate that the instance has been disposed.
                _disposed = true;
            }
        }
    }
}