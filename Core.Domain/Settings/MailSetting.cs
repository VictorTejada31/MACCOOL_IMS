namespace Core.Domain.Settings
{
    public class MailSetting
    {
        public string EmailFrom { get; set; }
        public string SmptUser { get; set; }
        public string SmptPass { get; set; }
        public int SmptPort { get; set; }
        public string SmptHost { get; set; }
        public string DisplayName { get; set; }

    }
}
