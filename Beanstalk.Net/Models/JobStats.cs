using YamlDotNet.Serialization;

namespace Beanstalk.Net.Models {
    public class JobStats {

        [YamlMember(Alias = "id")]
        public long Id { get; set; }

        [YamlMember(Alias = "tube")]
        public string Tube { get; set; }

        [YamlMember(Alias = "state")]
        public string State { get; set; }

        [YamlMember(Alias = "pri")]
        public uint Priority { get; set; }

        [YamlMember(Alias = "age")]
        public uint Age { get; set; }

        [YamlMember(Alias = "delay")]
        public uint Delay { get; set; }

        /// <summary>
        /// Number of seconds a worker is allowed to run this job
        /// </summary>
        [YamlMember(Alias = "ttr")]
        public uint TimeToRun { get; set; }

        /// <summary>
        /// Number of seconds left until the server puts this job into the ready queue.
        /// This number is only meaningful if the job is reserved or delayed.
        /// If the job is reserved and this amount of time elapses before its state
        /// changes, it is considered to have timed out.
        /// </summary>
        [YamlMember(Alias = "time-left")]
        public uint TimeLeft { get; set; }

        /// <summary>
        /// Number of the earliest binlog file containing this job. If -b wasn't used, this will be 0.
        /// </summary>
        [YamlMember(Alias = "file")]
        public uint File { get; set; }

        /// <summary>
        /// Number of times this job has been reserved.
        /// </summary>
        [YamlMember(Alias = "reserves")]
        public uint Reserves { get; set; }

        /// <summary>
        /// Number of times this job has timed out during a reservation.
        /// </summary>
        [YamlMember(Alias = "timeouts")]
        public uint Timeouts { get; set; }

        /// <summary>
        /// Number of times a client has released this job from a reservation.
        /// </summary>
        [YamlMember(Alias = "releases")]
        public uint Releases { get; set; }

        /// <summary>
        /// Number of times this job has been buried.
        /// </summary>
        [YamlMember(Alias = "buries")]
        public uint Buries { get; set; }

        /// <summary>
        /// Number of times this job has been kicked.
        /// </summary>
        [YamlMember(Alias = "kicks")]
        public uint Kicks { get; set; }
    }
}