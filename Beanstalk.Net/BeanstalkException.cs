using System;

namespace Beanstalk.Net {
    public class BeanstalkException : Exception {
        public BeanstalkException(string message) : base(message) { }
    }
}