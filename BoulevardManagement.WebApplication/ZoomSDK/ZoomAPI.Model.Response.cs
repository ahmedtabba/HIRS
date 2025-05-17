using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomAPI
{
    public class GlobalDialInNumber
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Room
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("participants")]
        public List<string> Participants { get; set; }
    }

    public class BreakoutRoom
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("rooms")]
        public List<Room> Rooms { get; set; }

        [JsonProperty("host_video")]
        public bool HostVideo { get; set; }

        [JsonProperty("in_meeting")]
        public bool InMeeting { get; set; }

        [JsonProperty("join_before_host")]
        public bool JoinBeforeHost { get; set; }

        [JsonProperty("mute_upon_entry")]
        public bool MuteUponEntry { get; set; }

        [JsonProperty("participant_video")]
        public bool ParticipantVideo { get; set; }

        [JsonProperty("registrants_confirmation_email")]
        public bool RegistrantsConfirmationEmail { get; set; }

        [JsonProperty("use_pmi")]
        public bool UsePmi { get; set; }

        [JsonProperty("waiting_room")]
        public bool WaitingRoom { get; set; }

        [JsonProperty("watermark")]
        public bool Watermark { get; set; }

        [JsonProperty("registrants_email_notification")]
        public bool RegistrantsEmailNotification { get; set; }
    }

    public class MeetingResponseSettings
    {
        [JsonProperty("alternative_hosts")]
        public string AlternativeHosts { get; set; }

        [JsonProperty("approval_type")]
        public int ApprovalType { get; set; }

        [JsonProperty("audio")]
        public string Audio { get; set; }

        [JsonProperty("auto_recording")]
        public string AutoRecording { get; set; }

        [JsonProperty("close_registration")]
        public bool CloseRegistration { get; set; }

        [JsonProperty("cn_meeting")]
        public bool CnMeeting { get; set; }

        [JsonProperty("enforce_login")]
        public bool EnforceLogin { get; set; }

        [JsonProperty("enforce_login_domains")]
        public string EnforceLoginDomains { get; set; }

        [JsonProperty("global_dial_in_countries")]
        public List<string> GlobalDialInCountries { get; set; }

        [JsonProperty("global_dial_in_numbers")]
        public List<GlobalDialInNumber> GlobalDialInNumbers { get; set; }

        [JsonProperty("breakout_room")]
        public BreakoutRoom BreakoutRoom { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("start_url")]
        public string StartUrl { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }

    public class MeetingResponse
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("join_before_host")]
        public bool JoinBeforeHost { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("start_url")]
        public string StartUrl { get; set; }
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("host_id")]
        public string HostId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("join_url")]
        public string JoinUrl { get; set; }

        [JsonProperty("settings")]
        public MeetingResponseSettings Settings { get; set; }
    }
}
