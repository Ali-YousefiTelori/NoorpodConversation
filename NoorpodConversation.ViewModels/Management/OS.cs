using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.ViewModels.Management
{
    /// <summary>
    /// کمک کننده ی وضعیت سیستم عامل
    /// </summary>
    public static class OSSystemInfo
    {
        /// <summary>
        /// آیا ویندوز ویستا و پایین تر از ان است؟
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsVistaAndLower()
        {
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            switch (vs.Major)
            {
                case 3:
                case 4:
                case 5:
                    return true;
                case 6:
                    {
                        if (vs.Minor == 0)
                            return true;
                        return false;
                    }
            }
            return false;
        }

        /// <summary>
        /// آیا ویندوز ایکس پی و پایین تر از ان است
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsXPAndLower()
        {
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;
            switch (vs.Major)
            {
                case 3:
                case 4:
                case 5:
                    return true;
            }
            return false;
        }
        /// <summary>
        /// دریافت نام سیستم عامل
        /// </summary>
        /// <returns></returns>
        public static string GetSystemInformation()
        {
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            switch (vs.Major)
            {
                case 3:
                    return "Windows NT 3.51";
                case 4:
                    return "Windows NT 4.0";
                case 5:
                    if (os.Version.Minor == 0)
                        return "Windows 2000";
                    else
                        return "Windows XP";
                case 6:
                    if (vs.Minor == 0)
                        return "Windows Vista";
                    else if (vs.Minor == 1)
                        return "Windows 7";
                    else if (vs.Minor == 2)
                        return "Windows 8";
                    else if (vs.Minor == 3)
                        return "Windows 8.1";
                    return "";
            }
            return "not found";
        }

    }
}
