using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomAPI
{
    public class Recurrence
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("repeat_interval")]
        public string RepeatInterval { get; set; }

        [JsonProperty("weekly_days")]
        public string WeeklyDays { get; set; }

        [JsonProperty("monthly_day")]
        public string MonthlyDay { get; set; }

        [JsonProperty("monthly_week")]
        public string MonthlyWeek { get; set; }

        [JsonProperty("monthly_week_day")]
        public string MonthlyWeekDay { get; set; }

        [JsonProperty("end_times")]
        public string EndTimes { get; set; }

        [JsonProperty("end_date_time")]
        public string EndDateTime { get; set; }
    }

    public class CreateMeetingSettings
    {
        [JsonProperty("host_video")]
        public string HostVideo { get; set; }

        [JsonProperty("participant_video")]
        public string ParticipantVideo { get; set; }

        [JsonProperty("cn_meeting")]
        public string CnMeeting { get; set; }

        [JsonProperty("in_meeting")]
        public string InMeeting { get; set; }

        [JsonProperty("join_before_host")]
        public bool JoinBeforeHost { get; set; }

        [JsonProperty("mute_upon_entry")]
        public string MuteUponEntry { get; set; }

        [JsonProperty("watermark")]
        public string Watermark { get; set; }

        [JsonProperty("use_pmi")]
        public string UsePmi { get; set; }

        [JsonProperty("approval_type")]
        public string ApprovalType { get; set; }

        [JsonProperty("registration_type")]
        public string RegistrationType { get; set; }

        [JsonProperty("audio")]
        public string Audio { get; set; }

        [JsonProperty("auto_recording")]
        public string AutoRecording { get; set; }

        [JsonProperty("enforce_login")]
        public string EnforceLogin { get; set; }

        [JsonProperty("enforce_login_domains")]
        public string EnforceLoginDomains { get; set; }

        [JsonProperty("alternative_hosts")]
        public string AlternativeHosts { get; set; }

        [JsonProperty("global_dial_in_countries")]
        public List<string> GlobalDialInCountries { get; set; }

        [JsonProperty("registrants_email_notification")]
        public string RegistrantsEmailNotification { get; set; }
    }

    public class CreateMeeting
    {
        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("schedule_for")]
        public string ScheduleFor { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("agenda")]
        public string Agenda { get; set; }

        [JsonProperty("recurrence")]
        public Recurrence Recurrence { get; set; }

        [JsonProperty("settings")]
        public CreateMeetingSettings Settings { get; set; }
    }




}
