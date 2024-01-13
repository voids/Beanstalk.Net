using YamlDotNet.Serialization;

namespace Beanstalk.Net.Models {
    public class TubeStats {
        /// <summary>
        /// the tube's name.
        /// </summary>
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Number of ready jobs with priority &lt; 1024 in this tube.
        /// </summary>
        [YamlMember(Alias = "current-jobs-urgent")]
        public uint CurrentJobsUrgent { get; set; }

        /// <summary>
        /// Number of jobs in the ready queue in this tube.
        /// </summary>
        [YamlMember(Alias = "current-jobs-ready")]
        public uint CurrentJobsReady { get; set; }

        /// <summary>
        /// Number of jobs reserved by all clients in this tube.
        /// </summary>
        [YamlMember(Alias = "current-jobs-reserved")]
        public uint CurrentJobsReserved { get; set; }

        /// <summary>
        /// Number of delayed jobs in this tube.
        /// </summary>
        [YamlMember(Alias = "current-jobs-delayed")]
        public uint CurrentJobsDelayed { get; set; }

        /// <summary>
        /// Number of buried jobs in this tube.
        /// </summary>
        [YamlMember(Alias = "current-jobs-buried")]
        public uint CurrentJobsBuried { get; set; }

        /// <summary>
        /// The cumulative count of jobs created in this tube in the current beanstalkd process.
        /// </summary>
        [YamlMember(Alias = "total-jobs")]
        public uint TotalJobs { get; set; }

        /// <summary>
        /// Number of open connections that are currently using this tube.
        /// </summary>
        [YamlMember(Alias = "current-using")]
        public uint CurrentUsing { get; set; }

        /// <summary>
        /// Number of open connections that have issued a reserve command while watching this tube but not yet received a response.
        /// </summary>
        [YamlMember(Alias = "current-waiting")]
        public uint CurrentWaiting { get; set; }

        /// <summary>
        /// Number of open connections that are currently watching this tube.
        /// </summary>
        [YamlMember(Alias = "current-watching")]
        public uint CurrentWatching { get; set; }

        /// <summary>
        /// Number of seconds the tube has been paused for.
        /// </summary>
        [YamlMember(Alias = "pause")]
        public uint Pause { get; set; }

        /// <summary>
        /// The cumulative number of delete commands for this tube
        /// </summary>
        [YamlMember(Alias = "cmd-delete")]
        public uint CmdDelete { get; set; }

        /// <summary>
        /// The cumulative number of pause-tube commands for this tube.
        /// </summary>
        [YamlMember(Alias = "cmd-pause-tube")]
        public uint CmdPauseTube { get; set; }

        /// <summary>
        /// The number of seconds until the tube is un-paused.
        /// </summary>
        [YamlMember(Alias = "pause-time-left")]
        public uint PauseTimeLeft { get; set; }
    }
}