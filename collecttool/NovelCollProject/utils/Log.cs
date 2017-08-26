using System;

namespace NovelCollProjectutils
{
    public class Log
    {
        public delegate void LOG_TXT(string txt);
        public event LOG_TXT OnLogText;

        public static bool ShowTime { get; set; }

        private static Log instance;
        public static Log GetInstance()
        {
            if (instance == null)
                instance = new Log();

            return instance;
        }

        public static string OutputFilePath { get; set; }

        public static void Show(object info)
        {
            ShowLine(info);
            //object t = TIME + info;
            //Console.Write(t);

            //if (GetInstance().OnLogText != null) GetInstance().OnLogText(t as string);
            //if (OutputFilePath != null) LocalFileIO.AppendAsUTF8(OutputFilePath, t.ToString());
        }

        public static void Show(object info, ConsoleColor fontColor)
        {
            ShowLine(info, fontColor);
            //Console.ForegroundColor = fontColor;
            //Show(info);
            //Console.ResetColor();
        }

        public static void ShowLine(object info)
        {
            object t = TIME + info;
            //t = info;

            Console.WriteLine(t);
            if (GetInstance().OnLogText != null) GetInstance().OnLogText(t+"\n");
            if (OutputFilePath != null) LocalFileIO.AppendAsUTF8(OutputFilePath, t + "\n\n");
        }

        public static void ShowLine(object info, ConsoleColor fontColor)
        {
            Console.ForegroundColor = fontColor;
            ShowLine(info);
            Console.ResetColor();
        }

        private static string TIME
        {
            get
            {
                if (!ShowTime) return "";
                DateTime date = DateTime.Now;
                return date.ToString("yyyy-MM-dd HH:mm:ss") + " ";
            }
        }
    }
}
