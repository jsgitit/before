namespace CodeLuau
{
	public class SpeakerRegistration
	{
		public int? SpeakerId { get; set; }
		public RegistrationStatus? Error { get; set; }

		public SpeakerRegistration(int speakerId)
		{
			this.SpeakerId = speakerId;
		}

		public SpeakerRegistration(RegistrationStatus? error)
		{
			this.Error = error;
		}

	}

	public enum RegistrationStatus
	{
		FirstNameRequired,
		LastNameRequired,
		EmailRequired,
		NoSessionsProvided,
		NoSessionsApproved,
		SpeakerDoesNotMeetStandards
	};
}
