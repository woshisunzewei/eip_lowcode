using System;

namespace EIP.Common.Log
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class LogWriter
    {
        /// <summary>
        /// 同样是记录信息，不过出现的频率要比Trace少一些，一般用来调试程序
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Debug(string message,string loggerName= "SystemFileDebugLog")
        {
            try
            {
                Logger logger = new Logger(loggerName);
                logger.Debug(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("日志记录失败:{0}", ex));
            }
        }

        /// <summary>
        /// 信息类型的消息
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message, string loggerName = "SystemFileInfoLog")
        {
            try
            {
                Logger logger = new Logger(loggerName);
                logger.Info(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("日志记录失败:{0}", ex));
            }
        }

        /// <summary>
        /// 警告信息，一般用于比较重要的场合
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warn(string message, string loggerName = "SystemFileWarnLog")
        {
            try
            {
                Logger logger = new Logger(loggerName);
                logger.Warn(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("日志记录失败:{0}", ex));
            }
        }

        /// <summary>
        /// 最常见的记录信息，一般用于普通输出
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Trace(string message, string loggerName = "SystemFileTraceLog")
        {
            try
            {
                Logger logger = new Logger(loggerName);
                logger.Trace(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("日志记录失败:{0}", ex));
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(string message, string loggerName = "SystemFileErrorLog")
        {
            try
            {
                Logger logger = new Logger(loggerName);
                logger.Error(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("日志记录失败:{0}", ex));
            }
        }

        /// <summary>
        /// 致命异常信息。一般来讲，发生致命异常之后程序将无法继续执行。
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Fatal(string message, string loggerName = "SystemFileFatalLog")
        {
            try
            {
                Logger logger = new Logger(loggerName);
                logger.Fatal(message);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("日志记录失败:{0}", ex));
            }
        }

    }
}