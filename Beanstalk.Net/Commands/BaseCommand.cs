using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beanstalk.Net.Commands {
    public abstract class BaseCommand : IBeanstalkCommand {

        /// <summary>
        /// Generate a Handler for "[respond]\r\n"
        /// </summary>
        /// <param name="func">user callback</param>
        /// <returns></returns>
        protected static Handler _g(Func<Task> func) {
            return async (args, stream) => {
                if (func == null) return;
                await func.Invoke();
            };
        }

        /// <summary>
        /// Generate a Handler for "[respond] [uint]\r\n"
        /// </summary>
        /// <param name="func">user callback</param>
        /// <returns></returns>
        protected static Handler _g(Func<uint, Task> func) {
            return async (args, stream) => {
                if (func == null) return;
                await func.Invoke(uint.Parse(args[1]));
            };
        }

        /// <summary>
        /// Generate a Handler for "[respond] [long]\r\n"
        /// </summary>
        /// <param name="func">user callback</param>
        /// <returns></returns>
        protected static Handler _g(Func<long, Task> func) {
            return async (args, stream) => {
                if (func == null) return;
                await func.Invoke(long.Parse(args[1]));
            };
        }

        /// <summary>
        /// Generate a Handler for "[respond] [string]\r\n"
        /// </summary>
        /// <param name="func">user callback</param>
        /// <returns></returns>
        protected static Handler _g(Func<string, Task> func) {
            return async (args, stream) => {
                if (func == null) return;
                await func.Invoke(args[1]);
            };
        }

        /// <summary>
        /// Generate a Handler for "[respond] [id] [size]\r\n[data]\r\n"
        /// </summary>
        /// <param name="func">user callback</param>
        /// <returns></returns>
        protected static Handler _g(Func<long, byte[], Task> func) {
            return async (args, stream) => {
                if (func == null) return;
                var id = long.Parse(args[1]);
                var len = uint.Parse(args[2]);
                var bytes = await stream.ReadBeanstalkBody(len);
                await func.Invoke(id, bytes);
            };
        }

        /// <summary>
        /// Generate a Handler for "OK [size]\r\n[yaml]\r\n"
        /// </summary>
        /// <param name="func">user callback</param>
        /// <returns></returns>
        protected static Handler _g(Func<List<string>, Task> func) {
            return async (args, stream) => {
                if (func == null) return;
                var len = uint.Parse(args[1]);
                var bytes = await stream.ReadBeanstalkBody(len);
                var str = BeanstalkConnection.Enc.GetString(bytes);
                await func.Invoke(BeanstalkConnection.YamlDz.Deserialize<List<string>>(str));
            };
        }

        protected readonly Dictionary<string, Handler> X = new Dictionary<string, Handler>();

        public void OnOutOfMemory(Func<Task> func) {
            X["OUT_OF_MEMORY"] = _g(func);
        }

        public void OnInternalError(Func<Task> func) {
            X["INTERNAL_ERROR"] = _g(func);
        }

        public void OnBadFormat(Func<Task> func) {
            X["BAD_FORMAT"] = _g(func);
        }

        public void OnUnknownCommand(Func<Task> func) {
            X["UNKNOWN_COMMAND"] = _g(func);
        }

        public IReadOnlyDictionary<string, Handler> Handlers => X;

        public abstract IEnumerable<byte[]> Provide();
    }
}