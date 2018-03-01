using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace IDCPictureMake
{
    class Program
    {
        static void Main(string[] args)
        {
            //string strImage = args[0];
            string strImageFront = "IdFront.png";
            string strImageBack = "IdBack.png";
            string strImageHead = ReadIniValue("IDInfo", "ImageHeadPath", "./IDCPictureMake.ini"); ;

            string strMakeFront = ReadIniValue("IDInfo", "MakeFront", "./IDCPictureMake.ini"); ;
            string strMakeBack = ReadIniValue("IDInfo", "MakeBack", "./IDCPictureMake.ini"); ;

            Bitmap bmpFront = new Bitmap(strImageFront);
            Bitmap bmpBack = new Bitmap(strImageBack);
            Bitmap bmpHead = new Bitmap(strImageHead);

            Graphics gFront = Graphics.FromImage(bmpFront);
            Graphics gBack = Graphics.FromImage(bmpBack);

            try
            {

                //字数太少识别报错，添加测试文字
                String strName = ReadIniValue("IDInfo", "Name", "./IDCPictureMake.ini");
                if (strName.Length == 2)
                {
                    strName = strName.Substring(0, 1) + " " + strName.Substring(1, 1);
                }

                String strSex = ReadIniValue("IDInfo", "Sex", "./IDCPictureMake.ini"); 
                String strNation = ReadIniValue("IDInfo", "Nation", "./IDCPictureMake.ini");

                String strBirthday = ReadIniValue("IDInfo", "Birthday", "./IDCPictureMake.ini");
                String strBirthdayY = strBirthday.Substring(0, 4);
                String strBirthdayM = strBirthday[4] == '0' ? strBirthday.Substring(5, 1) : strBirthday.Substring(4, 2);
                String strBirthdayD = strBirthday[6] == '0' ? strBirthday.Substring(7, 1) : strBirthday.Substring(6, 2);

                String strAddress = ReadIniValue("IDInfo", "Address", "./IDCPictureMake.ini"); 
                String strAddress1 = strAddress.Length > 11 ? strAddress.Substring(0, 11) : strAddress;
                String strAddress2 = strAddress.Length > 11 ? strAddress.Substring(10, strAddress.Length-10) : "";

                String strIdNo = ReadIniValue("IDInfo", "IdNo ", "./IDCPictureMake.ini");
                strIdNo = strIdNo[0] + " " + strIdNo[1] + " " + strIdNo[2] + " " + strIdNo[3] + " " +
                    strIdNo[4] + " " + strIdNo[5] + " " + strIdNo[6] + " " + strIdNo[7] + " " +
                    strIdNo[8] + " " + strIdNo[9] + " " + strIdNo[10] + " " + strIdNo[11] + " " +
                    strIdNo[12] + " " + strIdNo[13] + " " + strIdNo[14] + " " + strIdNo[15] + " " +
                    strIdNo[16] + " " + strIdNo[17];
                
                String strAuthority = ReadIniValue("IDInfo", "Authority", "./IDCPictureMake.ini"); 
                String strBegin = ReadIniValue("IDInfo", "Begin", "./IDCPictureMake.ini"); 
                String strEnd = ReadIniValue("IDInfo", "End", "./IDCPictureMake.ini"); 
                String strData = strBegin.Substring(0,4) + "." +strBegin.Substring(4,2) + "."+strBegin.Substring(6,2) +
                    "-" + strEnd.Substring(0, 4) + "." + strEnd.Substring(4, 2) + "." + strEnd.Substring(6, 2);


                Font font = new Font("方正黑体简体", 11);
                SolidBrush sbrush = new SolidBrush(Color.Black);
                gFront.DrawString(strName, font, sbrush, new PointF(80, 33));

                Font font1 = new Font("方正黑体简体", 10);
                gFront.DrawString(strSex, font1, sbrush, new PointF(80, 64));
                gFront.DrawString(strNation, font1, sbrush, new PointF(160, 64));

                gFront.DrawString(strBirthdayY, font1, sbrush, new PointF(80, 94));
                gFront.DrawString(strBirthdayM, font1, sbrush, new PointF(140, 94));
                gFront.DrawString(strBirthdayD, font1, sbrush, new PointF(175, 94));

                gFront.DrawString(strAddress1, font1, sbrush, new PointF(80, 128));
                gFront.DrawString(strAddress2, font1, sbrush, new PointF(80, 148));

                gBack.DrawString(strAuthority, font1, sbrush, new PointF(170, 180));
                gBack.DrawString(strData, font1, sbrush, new PointF(170, 212));

                Font font2 = new Font("华文细黑", 11, FontStyle.Bold);
                gFront.DrawString(strIdNo, font2, sbrush, new PointF(140, 205));

                bmpHead.MakeTransparent(Color.White);
                bmpHead = DelBackColour(bmpHead);
                gFront.DrawImage(bmpHead, new Rectangle(270, 50, bmpHead.Width, bmpHead.Height));

                bmpFront.Save(strMakeFront);
                bmpBack.Save(strMakeBack);

            }
            catch (System.Exception ex)
            {
            }
        }

        #region 去除头像白色背景
        /// <summary>  
        /// 去除头像白色背景  
        /// </summary>  
        /// <returns>处理后的图片</returns>  
        public static Bitmap DelBackColour(Bitmap tempImg)
        {
            Color __c;
            for (int x = 0; x < tempImg.Width; x++)
            {
                for (int y = 0; y < tempImg.Height; y++)
                {
                    __c = tempImg.GetPixel(x, y);
                    //灰度值  
                    int __tc = (__c.R + __c.G + __c.B) / 3;
                    //double luminance = (__c.R * 0.299) + (__c.G * 0.587) + (__c.B * 0.114);
                    //大于阙值 白色  
                    if (__tc > 220)
                    {
                        tempImg.SetPixel(x, y, Color.FromArgb(50, __c.R, __c.G, __c.B));
                    }
                    
                }
            }
            return tempImg;
        }
        #endregion 

        #region private IniFileInvoke dllImport

        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retval, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetTextCharacterExtra(IntPtr hdc, int nCharExtra);//图片字符间距

        #endregion
        /// <summary>
        /// 从ini文件中读取字符串值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public static string ReadIniValue(string section, string key, string sPath)
        {
            System.Text.StringBuilder steb = new System.Text.StringBuilder(255);
            GetPrivateProfileString(section, key, "", steb, 255, sPath);
            return steb.ToString();
        }
        /// <summary>
        /// 从ini文件中读取整型值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="skey"></param>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public static int ReadIniValueForInt(string section, string skey, string sPath)
        {
            return GetPrivateProfileInt(section, skey, -1, sPath);
        }
    }

    
}
