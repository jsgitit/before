using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeLuau
{
	/// <summary>
	/// Represents a single speaker
	/// </summary>
	public class Speaker
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? YearsOfExperience { get; set; }
		public bool HasBlog { get; set; }
		public string BlogURL { get; set; }
		public WebBrowser Browser { get; set; }
		public List<string> Certifications { get; set; }
		public string Employer { get; set; }
		public int RegistrationFee { get; set; }
		public List<Session> Sessions { get; set; }

		/// <summary>
		/// Register a speaker
		/// </summary>
		/// <returns>speakerID</returns>
		public SpeakerRegistration SetRegistrationStatus(IRepository repository)
        {
            var error = ValidateRegistration();
            if (error != null) return new SpeakerRegistration(error);
            int? speakerId = repository.SaveSpeaker(this);
            return new SpeakerRegistration((int)speakerId);
        }

        private RegistrationStatus? ValidateRegistration()
        {
            var error = ValidateData();
            if (error != null) return error;
           
            bool speakerIsQualified = SpeakerAppearsExceptional() || !HasObviousRedFlags();
            if (!speakerIsQualified) return RegistrationStatus.SpeakerDoesNotMeetStandards;
            
            bool atLeastOneSessionApproved = isOneSessionApproved();
            if (!atLeastOneSessionApproved) return RegistrationStatus.NoSessionsApproved;
            
            return null;
        }

        private bool isOneSessionApproved()
        {
            foreach (var sessionTopic in Sessions)
            {
                sessionTopic.Approved = !SessionIsAboutOldTechnology(sessionTopic);
            }

            return Sessions.Any(s => s.Approved);
        }
       
        private bool SessionIsAboutOldTechnology(Session sessionTopic)
        {
            var oldTechnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
            foreach (var tech in oldTechnologies)
            {
                if (sessionTopic.Title.Contains(tech) || sessionTopic.Description.Contains(tech))
                {
                    return true;
                }
            }
            return false;
        }
        
        private bool HasObviousRedFlags()
        {
            //need to get just the domain from the email
            string emailDomain = Email.Split('@').Last();

                
            var ancientEmailDomains = new List<string>() { "aol.com", "prodigy.com", "compuserve.com" };
            if (ancientEmailDomains.Contains(emailDomain)) return true;
            if (Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9) return true;
            return false;
        }

        private bool SpeakerAppearsExceptional()
        {
            if (YearsOfExperience > 10) return true;
            if (HasBlog) return true;
            if (Certifications.Count() > 3) return true;
            
            var preferredEmployers = new List<string>() { "Pluralsight", "Microsoft", "Google" };
            if (preferredEmployers.Contains(Employer)) return true;
            return false;
        }

        private RegistrationStatus? ValidateData()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return RegistrationStatus.FirstNameRequired;
            if (string.IsNullOrWhiteSpace(LastName)) return RegistrationStatus.LastNameRequired;
            if (string.IsNullOrWhiteSpace(Email)) return RegistrationStatus.EmailRequired;
            if (!Sessions.Any()) return RegistrationStatus.NoSessionsProvided;
            return null;
        }
	}
}