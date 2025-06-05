// <copyright file="XMLDatabase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WSUFundraiserEngine
{
    using System.Xml;

    /// <summary>
    /// This class is responsible for saving and loading the Fundraiser data from an XML file.
    /// </summary>
    public static class XMLDatabase
    {
        /*
         Database Structure:
        <WSUFundraiser>

          <Users>

            <User>
              <Type>Student</Type>
              <Name>John Doe</Name>
              <Email>john@example.com</Email>
              <Username>johndoe</Username>
              <Password>password123</Password>
              <WsuId>WSU123</WsuId>

              <Clubs>
                <Club>Robotics Club</Club>
                <Club>Art Club</Club>
              </Clubs>
            </User>

            <User>
              <Type>Donor</Type>
              <Name>Jane Smith</Name>
              <Email>jane@example.com</Email>
              <Username>janesmith</Username>
              <Password>password456</Password>
              <DonorId>D12345</DonorId>
              <Balance>2000</Balance>

              <Donations>
                <Donation>
                  <ProjectName>Community Garden</ProjectName>
                  <Amount>500</Amount>
                  <Date>2024-12-02</Date>
                  <IsAnonymous>false</IsAnonymous>
                </Donation>
              </Donations>
            </User>

          </Users>

        <Projects>
            <Project>
              <Name>Community Garden</Name>
              <Description>Help fund a local garden.</Description>
              <TargetAmount>1000</TargetAmount>
              <RaisedAmount>500</RaisedAmount>
              <EndDate>2025-01-01</EndDate>
              <Members>
                <Member>John Doe</Member>
              </Members>
            </Project>
          </Projects>

        <Scholarships>
            <Scholarship>
              <Name>STEM Scholarship</Name>
              <Description>Scholarship for STEM students.</Description>
              <Status>Open</Status>
              <Criteria>
                <Criterion>GPA > 3.5</Criterion>
                <Criterion>Major: STEM</Criterion>
              </Criteria>
              <AwardedStudents>
                <Student>John Doe</Student>
                <Student>Jane Smith</Student>
              </AwardedStudents>
            </Scholarship>
        </Scholarships>

        </WSUFundraiser>
         */
        private const string FilePath = @"C:\Users\Ehiso\OneDrive\Desktop\WSU CLASSES\Senior_1st_Semester\CPTS 321\Coding\inClassActivities\cpts321-ehiane_oigiagbe-in-class-execises\WSUFundraiserEngine\WSUFundraiserDatabase.xml";
        //private const string FilePath = @"C:\Users\Ehiso\OneDrive\Desktop\WSU CLASSES\Senior_1st_Semester\CPTS 321\Coding\inClassActivities\cpts321-ehiane_oigiagbe-in-class-execises\WSUFundraiserTests\WSUFundraiserTestDatabase.xml";

        /// <summary>
        /// This method loads the users from the XML file.
        /// </summary>
        /// <param name="registeredUsers">The list of registers users to be saved</param>
        public static void SaveUsers(List<User> registeredUsers)
        {
            XmlDocument doc = new XmlDocument();

            if (File.Exists(FilePath))
            {
                doc.Load(FilePath);
            }
            else
            {
                XmlElement root = doc.CreateElement("WSUFundraiser");
                doc.AppendChild(root);
            }

            XmlElement usersElement = doc.SelectSingleNode("//Users") as XmlElement;
            if (usersElement == null)
            {
                usersElement = doc.CreateElement("Users");
                doc.DocumentElement.AppendChild(usersElement);
            }

            foreach (var user in registeredUsers)
            {
                XmlElement existingUserElement = usersElement.SelectSingleNode($"User[Username='{user.Username}']") as XmlElement;

                if (existingUserElement != null)
                {
                    // Update existing user
                    existingUserElement.SelectSingleNode("Name").InnerText = user.Name;
                    existingUserElement.SelectSingleNode("Email").InnerText = user.Email;
                    existingUserElement.SelectSingleNode("Password").InnerText = user.Password;

                    if (user is Student student)
                    {
                        existingUserElement.SelectSingleNode("WsuId").InnerText = student.WsuId;
                        existingUserElement.SelectSingleNode("Major").InnerText = student.Major;
                        existingUserElement.SelectSingleNode("GraduationYear").InnerText = student.GraduationYear;

                        XmlElement clubsElement = existingUserElement.SelectSingleNode("Clubs") as XmlElement;
                        clubsElement.RemoveAll();
                        foreach (var club in student.Clubs)
                        {
                            XmlElement clubElement = doc.CreateElement("Club");
                            clubElement.AppendChild(CreateElement(doc, "Name", club.Name));
                            clubElement.AppendChild(CreateElement(doc, "Description", club.Description));
                            clubElement.AppendChild(CreateElement(doc, "StartDate", club.StartDate.ToString("yyyy-MM-dd")));
                            clubElement.AppendChild(CreateElement(doc, "EndDate", club.EndDate.ToString("yyyy-MM-dd")));
                            clubsElement.AppendChild(clubElement);
                        }
                    }

                    if (user is Donor donor)
                    {
                        existingUserElement.SelectSingleNode("DonorId").InnerText = donor.DonorId;
                        existingUserElement.SelectSingleNode("Balance").InnerText = donor.GetDonorBalance().ToString();

                        XmlElement donationsElement = existingUserElement.SelectSingleNode("Donations") as XmlElement;
                        donationsElement.RemoveAll();
                        foreach (var donation in donor.Donations)
                        {
                            XmlElement donationElement = doc.CreateElement("Donation");
                            donationElement.AppendChild(CreateElement(doc, "ProjectName", donation.Project.Name));
                            donationElement.AppendChild(CreateElement(doc, "Amount", donation.Amount.ToString()));
                            donationElement.AppendChild(CreateElement(doc, "Date", donation.Date.ToString("yyyy-MM-dd")));
                            donationElement.AppendChild(CreateElement(doc, "IsAnonymous", donation.IsAnonymous.ToString()));
                            donationsElement.AppendChild(donationElement);
                        }
                    }
                }
                else
                {
                    // Add a new user
                    XmlElement userElement = doc.CreateElement("User");
                    userElement.SetAttribute("Type", user.GetType().Name);
                    userElement.AppendChild(CreateElement(doc, "Name", user.Name));
                    userElement.AppendChild(CreateElement(doc, "Email", user.Email));
                    userElement.AppendChild(CreateElement(doc, "Username", user.Username));
                    userElement.AppendChild(CreateElement(doc, "Password", user.Password));

                    if (user is Student student)
                    {
                        userElement.AppendChild(CreateElement(doc, "WsuId", student.WsuId));
                        userElement.AppendChild(CreateElement(doc, "Major", student.Major));
                        userElement.AppendChild(CreateElement(doc, "GraduationYear", student.GraduationYear));

                        XmlElement clubsElement = doc.CreateElement("Clubs");
                        foreach (var club in student.Clubs)
                        {
                            XmlElement clubElement = doc.CreateElement("Club");
                            clubElement.AppendChild(CreateElement(doc, "Name", club.Name));
                            clubElement.AppendChild(CreateElement(doc, "Description", club.Description));
                            clubElement.AppendChild(CreateElement(doc, "StartDate", club.StartDate.ToString("yyyy-MM-dd")));
                            clubElement.AppendChild(CreateElement(doc, "EndDate", club.EndDate.ToString("yyyy-MM-dd")));
                            clubsElement.AppendChild(clubElement);
                        }

                        userElement.AppendChild(clubsElement);
                    }

                    if (user is Donor donor)
                    {
                        userElement.AppendChild(CreateElement(doc, "DonorId", donor.DonorId));
                        userElement.AppendChild(CreateElement(doc, "Balance", donor.GetDonorBalance().ToString()));

                        XmlElement donationsElement = doc.CreateElement("Donations");
                        foreach (var donation in donor.Donations)
                        {
                            XmlElement donationElement = doc.CreateElement("Donation");
                            donationElement.AppendChild(CreateElement(doc, "ProjectName", donation.Project.Name));
                            donationElement.AppendChild(CreateElement(doc, "Amount", donation.Amount.ToString()));
                            donationElement.AppendChild(CreateElement(doc, "Date", donation.Date.ToString("yyyy-MM-dd")));
                            donationElement.AppendChild(CreateElement(doc, "IsAnonymous", donation.IsAnonymous.ToString()));
                            donationsElement.AppendChild(donationElement);
                        }

                        userElement.AppendChild(donationsElement);
                    }

                    usersElement.AppendChild(userElement);
                }
            }

            doc.Save(FilePath);
        }

        /// <summary>
        /// This method loads the users from the XML file.
        /// </summary>
        /// <returns>This returns the list of users saved in the application.</returns>
        public static List<User> LoadUsers()
        {
            List<User> users = new List<User>();
            XmlDocument doc = new XmlDocument();

            if (!File.Exists(FilePath))
            {
                return users; // Return an empty list if the file does not exist
            }

            doc.Load(FilePath);
            XmlNodeList userNodes = doc.SelectNodes("//Users/User");

            foreach (XmlNode userNode in userNodes)
            {
                string type = userNode.Attributes["Type"].Value;
                string name = userNode.SelectSingleNode("Name").InnerText;
                string email = userNode.SelectSingleNode("Email").InnerText;
                string username = userNode.SelectSingleNode("Username").InnerText;
                string password = userNode.SelectSingleNode("Password").InnerText;

                if (type == "Student")
                {
                    string wsuId = userNode.SelectSingleNode("WsuId").InnerText;
                    string major = userNode.SelectSingleNode("Major").InnerText;
                    string graduationYear = userNode.SelectSingleNode("GraduationYear").InnerText;

                    List<Club> clubs = new List<Club>();
                    XmlNodeList clubNodes = userNode.SelectNodes("Clubs/Club");

                    foreach (XmlNode clubNode in clubNodes)
                    {
                        string clubName = clubNode["Name"]?.InnerText;
                        string clubDescription = clubNode["Description"]?.InnerText;
                        clubs.Add(new Club(clubName, clubDescription));
                    }

                    var student = new Student(name, email, username, password, major, wsuId, graduationYear);
                    foreach (var club in clubs)
                    {
                        student.JoinClub(club);
                    }

                    users.Add(student);
                }

                if (type == "Donor")
                {
                    string donorId = userNode.SelectSingleNode("DonorId").InnerText;
                    decimal balance = decimal.Parse(userNode.SelectSingleNode("Balance").InnerText);
                    List<Donation> donations = new List<Donation>();
                    XmlNodeList donationNodes = userNode.SelectNodes("Donations/Donation");

                    var donor = new Donor(name, email, username, password, donorId)
                    {
                        Donations = donations,
                    };

                    donor.Deposit(balance);

                    foreach (XmlNode donationNode in donationNodes)
                    {
                        string projectName = donationNode.SelectSingleNode("ProjectName").InnerText;
                        decimal amount = decimal.Parse(donationNode.SelectSingleNode("Amount").InnerText);
                        DateTime date = DateTime.Parse(donationNode.SelectSingleNode("Date").InnerText);
                        bool isAnonymous = bool.Parse(donationNode.SelectSingleNode("IsAnonymous").InnerText);

                        // will handle project creation later. 
                        donations.Add(new Donation(null, new Project(projectName, null, 0), amount, isAnonymous));
                    }

                    users.Add(donor);
                }
            }

            return users;
        }

        /// <summary>
        /// This method saves the projects to the XML file.
        /// </summary>
        /// <param name="newProjects">The project to be saved.</param>
        public static void SaveProjects(List<Project> newProjects)
        {
            XmlDocument doc = new XmlDocument();

            if (File.Exists(FilePath))
            {
                doc.Load(FilePath);
            }
            else
            {
                XmlElement root = doc.CreateElement("WSUFundraiser");
                doc.AppendChild(root);
            }

            XmlElement projectsElement = doc.SelectSingleNode("//Projects") as XmlElement;
            if (projectsElement == null)
            {
                projectsElement = doc.CreateElement("Projects");
                doc.DocumentElement.AppendChild(projectsElement);
            }

            foreach (var project in newProjects)
            {
                XmlElement existingProject = projectsElement.SelectSingleNode($"Project[Name='{project.Name}']") as XmlElement;
                if (existingProject != null)
                {
                    projectsElement.RemoveChild(existingProject); // Remove the old entry
                }

                XmlElement projectElement = doc.CreateElement("Project");
                projectElement.AppendChild(CreateElement(doc, "Name", project.Name));
                projectElement.AppendChild(CreateElement(doc, "Description", project.Description));
                projectElement.AppendChild(CreateElement(doc, "TargetAmount", project.TargetAmount.ToString()));
                projectElement.AppendChild(CreateElement(doc, "RaisedAmount", project.RaisedAmount.ToString()));
                projectElement.AppendChild(CreateElement(doc, "StartDate", project.StartDate.ToString("yyyy-MM-dd")));
                projectElement.AppendChild(CreateElement(doc, "EndDate", project.EndDate.ToString("yyyy-MM-dd")));

                // Add members to the project
                XmlElement membersElement = doc.CreateElement("Members");
                foreach (var member in project.Members)
                {
                    XmlElement memberElement = doc.CreateElement("Member");
                    memberElement.InnerText = member.Username; // Use a unique identifier like username
                    membersElement.AppendChild(memberElement);
                }

                projectElement.AppendChild(membersElement);

                // Add the project to the projects element

                projectsElement.AppendChild(projectElement);
            }

            doc.Save(FilePath);
        }

        /// <summary>
        /// This method loads the projects from the XML file.
        /// </summary>
        /// <returns>The list of projects saved to the file.</returns>
        public static List<Project> LoadProjects()
        {
            List<Project> projects = new List<Project>();

            // Check if the XML file exists
            if (!File.Exists(FilePath))
            {
                return projects; // Return an empty list if the file doesn't exist
            }

            // Load the XML file
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);

            // Locate the project nodes in the XML structure
            XmlNodeList projectNodes = doc.SelectNodes("//Projects/Project");

            // Parse each project node
            foreach (XmlNode projectNode in projectNodes)
            {
                string name = projectNode["Name"]?.InnerText ?? "Unnamed Project";
                string description = projectNode["Description"]?.InnerText ?? "No Description Available";
                decimal targetAmount = decimal.Parse(projectNode["TargetAmount"]?.InnerText ?? "0");
                decimal raisedAmount = decimal.Parse(projectNode["RaisedAmount"]?.InnerText ?? "0");
                DateTime startDate = DateTime.Parse(projectNode["StartDate"]?.InnerText ?? DateTime.MinValue.ToString());
                DateTime endDate = DateTime.Parse(projectNode["EndDate"]?.InnerText ?? DateTime.MaxValue.ToString());

                // Create a Project object and add it to the list
                Project project = new Project(name, description, targetAmount)
                {
                    RaisedAmount = raisedAmount,
                    StartDate = startDate,
                    EndDate = endDate,
                };

                // Load project members
                XmlNodeList memberNodes = projectNode.SelectNodes("//Members/Member");
                foreach (XmlNode memberNode in memberNodes)
                {
                    string username = memberNode.InnerText;
                    Student student = GetStudentByUsername(username); // Helper method to fetch student by username
                    if (student != null)
                    {
                        project.AddMember(student);
                    }
                }

                projects.Add(project);
            }

            return projects;
        }

        /// <summary>
        /// This method saves the scholarships to the XML file.
        /// </summary>
        /// <param name="newScholarships">The scholarships to be saved.</param>
        public static void SaveScholarships(List<Scholarship> newScholarships)
        {
            XmlDocument doc = new XmlDocument();

            if (File.Exists(FilePath))
            {
                doc.Load(FilePath);
            }
            else
            {
                XmlElement root = doc.CreateElement("WSUFundraiser");
                doc.AppendChild(root);
            }

            XmlElement scholarshipsElement = doc.SelectSingleNode("//Scholarships") as XmlElement;
            if (scholarshipsElement == null)
            {
                scholarshipsElement = doc.CreateElement("Scholarships");
                doc.DocumentElement.AppendChild(scholarshipsElement);
            }

            foreach (var scholarship in newScholarships)
            {
                XmlElement existingScholarship = scholarshipsElement.SelectSingleNode($"Scholarship[Name='{scholarship.Name}']") as XmlElement;
                if (existingScholarship != null)
                {
                    scholarshipsElement.RemoveChild(existingScholarship); // Remove the old entry
                }

                XmlElement scholarshipElement = doc.CreateElement("Scholarship");
                scholarshipElement.AppendChild(CreateElement(doc, "Name", scholarship.Name));
                scholarshipElement.AppendChild(CreateElement(doc, "Description", scholarship.Description));
                scholarshipElement.AppendChild(CreateElement(doc, "Status", scholarship.Status.ToString()));

                XmlElement criteriaElement = doc.CreateElement("Criteria");
                foreach (var criterion in scholarship.Criteria)
                {
                    criteriaElement.AppendChild(CreateElement(doc, "Criterion", criterion));
                }
                scholarshipElement.AppendChild(criteriaElement);

                XmlElement applicantsElement = doc.CreateElement("Applicants");
                foreach (var applicant in scholarship.Applicants)
                {
                    applicantsElement.AppendChild(CreateElement(doc, "Applicant", applicant.Username));
                }
                scholarshipElement.AppendChild(applicantsElement);

                XmlElement awardedStudentsElement = doc.CreateElement("AwardedStudents");
                foreach (var student in scholarship.AwardedStudents)
                {
                    awardedStudentsElement.AppendChild(CreateElement(doc, "Student", student.Username));
                }
                scholarshipElement.AppendChild(awardedStudentsElement);

                scholarshipsElement.AppendChild(scholarshipElement);
            }

            doc.Save(FilePath);
        }

        /// <summary>
        /// This method loads the scholarships from the XML file.
        /// </summary>
        /// <returns>The list of scholarships to be added.</returns>
        public static List<Scholarship> LoadScholarships()
        {
            List<Scholarship> scholarships = new List<Scholarship>();

            if (!File.Exists(FilePath))
            {
                return scholarships;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);

            // Locate the scholarship nodes in the XML structure
            XmlNodeList scholarshipNodes = doc.SelectNodes("//Scholarships/Scholarship");

            // Parse each scholarship node
            foreach (XmlNode scholarshipNode in scholarshipNodes)
            {
                string name = scholarshipNode["Name"]?.InnerText ?? "Unnamed Scholarship";
                string description = scholarshipNode["Description"]?.InnerText ?? "No Description Available";
                string status = scholarshipNode["Status"]?.InnerText ?? "false";

                // Load the criteria
                List<string> criteria = scholarshipNode.SelectNodes("Criteria/Criterion")
                                                       .Cast<XmlNode>()
                                                       .Select(node => node.InnerText)
                                                       .ToList();

                // Load Applicants
                List<Student> applicants = scholarshipNode.SelectNodes("Applicants/Applicant")
                                                          .Cast<XmlNode>()
                                                          .Select(node =>
                                                          {
                                                              string username = node.InnerText;

                                                              return GetStudentByUsername(username);
                                                          })
                                                          .ToList();

                // Load Awarded Students
                List<Student> awardedStudents = scholarshipNode.SelectNodes("AwardedStudents/Student")
                                                                .Cast<XmlNode>()
                                                                .Select(node =>
                                                                {
                                                                    string name = node["Name"]?.InnerText ?? "Unknown Name";
                                                                    string email = node["Email"]?.InnerText ?? "Unknown Email";
                                                                    string username = node["Username"]?.InnerText ?? "Unknown Username";
                                                                    string password = node["Password"]?.InnerText ?? "Unknown Password";
                                                                    string major = node["Major"]?.InnerText ?? "Unknown Major";
                                                                    string wsuId = node["WsuId"]?.InnerText ?? "Unknown WSU ID";
                                                                    string graduationYear = node["GraduationYear"]?.InnerText ?? "Unknown Year";

                                                                    // Create a new student object
                                                                    return new Student(name, email, username, password, major, wsuId, graduationYear);
                                                                })
                                                                .ToList();

                // Create a new scholarship object
                Scholarship scholarship = new Scholarship(name, description, status, criteria);

                // Add the applicants to the scholarship
                foreach (var applicant in applicants)
                {
                    scholarship.Apply(applicant);
                }

                // Award the students
                foreach (var student in awardedStudents)
                {
                    scholarship.Award(student);
                }

                scholarships.Add(scholarship);
            }

            return scholarships;
        }

        /// <summary>
        /// This method saves the clubs to the XML file, appending new clubs and updating existing ones.
        /// </summary>
        /// <param name="newClubs">The list of clubs to be saved to the database.</param>
        public static void SaveClubs(List<Club> newClubs)
        {
            XmlDocument doc = new XmlDocument();

            // Load or create the XML document
            if (File.Exists(FilePath))
            {
                doc.Load(FilePath);
            }
            else
            {
                XmlElement root = doc.CreateElement("WSUFundraiser");
                doc.AppendChild(root);
            }

            // Get or create the Clubs element
            XmlElement clubsElement = doc.SelectSingleNode("//Clubs") as XmlElement;
            if (clubsElement == null)
            {
                clubsElement = doc.CreateElement("Clubs");
                doc.DocumentElement.AppendChild(clubsElement);
            }

            foreach (var club in newClubs)
            {
                // Check if the club already exists
                XmlElement existingClubElement = clubsElement.SelectSingleNode($"Club[Name='{club.Name}']") as XmlElement;

                if (existingClubElement != null)
                {
                    // Update the existing club
                    UpdateClubElement(existingClubElement, club, doc);
                }
                else
                {
                    // Create a new club element
                    XmlElement newClubElement = CreateClubElement(club, doc);
                    clubsElement.AppendChild(newClubElement);
                }
            }

            doc.Save(FilePath);
        }

        /// <summary>
        /// This method cleans the database by deleting the database file.
        /// </summary>
        public static void CleanDatabase()
        {
            // Delete the database file
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        /// <summary>
        /// This method loads the clubs from the XML file.
        /// </summary>
        /// <returns>All saved clubs details.</returns>
        public static List<Club> LoadClubs()
        {
            List<Club> clubs = new List<Club>();
            XmlDocument doc = new XmlDocument();

            if (!File.Exists(FilePath))
            {
                return clubs;
            }

            doc.Load(FilePath);
            XmlNodeList clubNodes = doc.SelectNodes("//Clubs/Club");

            foreach (XmlNode clubNode in clubNodes)
            {
                string clubName = clubNode["Name"]?.InnerText;
                string clubDescription = clubNode["Description"]?.InnerText;
                DateTime startDate = DateTime.Parse(clubNode["StartDate"]?.InnerText ?? DateTime.MinValue.ToString());
                DateTime endDate = DateTime.Parse(clubNode["EndDate"]?.InnerText ?? DateTime.MaxValue.ToString());
                Club club = new Club(clubName, clubDescription);
                club.StartDate = startDate;
                club.EndDate = endDate;

                XmlNodeList memberNodes = clubNode.SelectNodes("//Members/Member");
                foreach (XmlNode memberNode in memberNodes)
                {
                    string username = memberNode.InnerText;
                    Student student = GetStudentByUsername(username); // Helper method to fetch student by username
                    if (student != null)
                    {
                        club.AddMember(student);
                    }
                }

                XmlNodeList projectNodes = clubNode.SelectNodes("Projects/Project");
                foreach (XmlNode projectNode in projectNodes)
                {
                    string projectName = projectNode["Name"]?.InnerText;
                    string projectDescription = projectNode["Description"]?.InnerText;
                    DateTime projectStartDate = DateTime.Parse(projectNode["StartDate"]?.InnerText ?? DateTime.MinValue.ToString());
                    DateTime projectEndDate = DateTime.Parse(projectNode["EndDate"]?.InnerText ?? DateTime.MaxValue.ToString());
                    Project project = new Project(projectName, projectDescription, 0);
                    project.StartDate = projectStartDate;
                    project.EndDate = projectEndDate;
                    club.AddProject(project);
                }

                clubs.Add(club);
            }

            return clubs;
        }

        private static Student GetStudentByUsername(string username)
        {
            // Load all users from the database
            List<User> users = LoadUsers();

            // Search for a student with the matching username
            foreach (var user in users)
            {
                if (user is Student student && student.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    return student;
                }
            }

            // Return null if no matching student is found
            return null;
        }

        /// <summary>
        /// This method creates an XML element with the specified name and value.
        /// </summary>
        /// <param name="doc">The xml document to be modified.</param>
        /// <param name="name">The name of the element to be added to the element.</param>
        /// <param name="value">The value of the element (innertext).</param>
        /// <returns>An XML element that contains the details of the arguments.</returns>
        private static XmlElement CreateElement(XmlDocument doc, string name, string value)
        {
            XmlElement element = doc.CreateElement(name);
            element.InnerText = value;
            return element;
        }

        /// <summary>
        /// Updates an existing club element with new details.
        /// </summary>
        /// <param name="clubElement">The existing XML element representing the club.</param>
        /// <param name="club">The club object with updated details.</param>
        /// <param name="doc">The XML document.</param>
        private static void UpdateClubElement(XmlElement clubElement, Club club, XmlDocument doc)
        {
            clubElement.SelectSingleNode("Description").InnerText = club.Description;

            // Update or create the Members node
            XmlElement membersElement = clubElement.SelectSingleNode("Members") as XmlElement;
            if (membersElement == null)
            {
                membersElement = doc.CreateElement("Members");
                clubElement.AppendChild(membersElement);
            }
            else
            {
                membersElement.RemoveAll(); // Clear existing members
            }

            foreach (var member in club.Members)
            {
                membersElement.AppendChild(CreateElement(doc, "Member", member.Username));
            }

            // Update or create the Projects node
            XmlElement projectsElement = clubElement.SelectSingleNode("Projects") as XmlElement;
            if (projectsElement == null)
            {
                projectsElement = doc.CreateElement("Projects");
                clubElement.AppendChild(projectsElement);
            }
            else
            {
                projectsElement.RemoveAll(); // Clear existing projects
            }

            foreach (var project in club.Projects)
            {
                XmlElement projectElement = doc.CreateElement("Project");
                projectElement.AppendChild(CreateElement(doc, "Name", project.Name));
                projectElement.AppendChild(CreateElement(doc, "Description", project.Description));
                projectElement.AppendChild(CreateElement(doc, "TargetAmount", project.TargetAmount.ToString()));
                projectElement.AppendChild(CreateElement(doc, "RaisedAmount", project.RaisedAmount.ToString()));
                projectElement.AppendChild(CreateElement(doc, "StartDate", project.StartDate.ToString("yyyy-MM-dd")));
                projectElement.AppendChild(CreateElement(doc, "EndDate", project.EndDate.ToString("yyyy-MM-dd")));
                projectsElement.AppendChild(projectElement);
            }
        }

        /// <summary>
        /// Creates a new club element for the XML file.
        /// </summary>
        /// <param name="club">The club object to convert to an XML element.</param>
        /// <param name="doc">The XML document.</param>
        /// <returns>An XML element representing the club.</returns>
        private static XmlElement CreateClubElement(Club club, XmlDocument doc)
        {
            XmlElement clubElement = doc.CreateElement("Club");

            clubElement.AppendChild(CreateElement(doc, "Name", club.Name));
            clubElement.AppendChild(CreateElement(doc, "Description", club.Description));
            clubElement.AppendChild(CreateElement(doc, "StartDate", club.StartDate.ToString("yyyy-MM-dd")));
            clubElement.AppendChild(CreateElement(doc, "EndDate", club.EndDate.ToString("yyyy-MM-dd")));

            // Add Members
            XmlElement membersElement = doc.CreateElement("Members");
            foreach (var member in club.Members)
            {
                membersElement.AppendChild(CreateElement(doc, "Member", member.Username));
            }
            clubElement.AppendChild(membersElement);

            // Add Projects
            XmlElement projectsElement = doc.CreateElement("Projects");
            foreach (var project in club.Projects)
            {
                XmlElement projectElement = doc.CreateElement("Project");
                projectElement.AppendChild(CreateElement(doc, "Name", project.Name));
                projectElement.AppendChild(CreateElement(doc, "Description", project.Description));
                projectElement.AppendChild(CreateElement(doc, "TargetAmount", project.TargetAmount.ToString()));
                projectElement.AppendChild(CreateElement(doc, "RaisedAmount", project.RaisedAmount.ToString()));
                projectElement.AppendChild(CreateElement(doc, "StartDate", project.StartDate.ToString("yyyy-MM-dd")));
                projectElement.AppendChild(CreateElement(doc, "EndDate", project.EndDate.ToString("yyyy-MM-dd")));
                projectsElement.AppendChild(projectElement);
            }
            clubElement.AppendChild(projectsElement);

            return clubElement;
        }

    }
}
