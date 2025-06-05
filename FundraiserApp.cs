
namespace WSUFundraiser
{
    using WSUFundraiserEngine;

    /// <summary>
    /// This class represents the main form of the application.
    /// </summary>
    public partial class FundraiserApp : Form
    {
        /// <summary>
        /// This field represents the current panel being displayed.
        /// </summary>
        private Panel currentPanel;

        private Stack<Panel> navigationStack = new Stack<Panel>();

        /// <summary>
        /// This list contains all the projects in the system.
        /// </summary>
        private List<Project> projects = new List<Project>();

        /// <summary>
        /// This list contains all the scholarships in the system.
        /// </summary>
        private List<Scholarship> scholarships = new List<Scholarship>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FundraiserApp"/> class.
        /// </summary>
        public FundraiserApp()
        {
            this.InitializeComponent();
            this.HideAllViews();
            this.InitializeGuestDashboard();
            this.GuestComboBox_Projects.SelectedIndexChanged += this.GuestComboBox_Projects_SelectedIndexChanged;
            this.GuestComboBox_Scholarships.SelectedIndexChanged += this.GuestComboBox_Scholarships_SelectedIndexChanged;
        }

        /// <summary>
        /// This method shows the selected panel, Acts like the routing mechanism.
        /// </summary>
        /// <param name="panel">The windoe to be displayed.</param>
        private void ShowPanel(Panel panel)
        {
            if (this.currentPanel != null)
            {
                this.currentPanel.Visible = false;
            }

            this.navigationStack.Push(panel);

            // Show the new panel
            panel.Visible = true;
            panel.BringToFront();

            // Set the current panel
            this.currentPanel = panel;
        }

        private void HideAllViews()
        {
            this.GuestPanel.Visible = false;
            this.RegisterPanel.Visible = false;
            this.StudentView.Visible = false;
            //this.LoginPanel.Visible = false;
        }

        private void LoadProjects()
        {
            this.projects = XMLDatabase.LoadProjects();
        }

        private void LoadScholarships()
        {
            this.scholarships = XMLDatabase.LoadScholarships();
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

        private void GuestButton_Click(object? sender, EventArgs e)
        {
            try
            {
                User guestUser = UserFactory.CreateUser("guest", "Guest", null, null, null, null);

                // redirect to the dashboard
                this.ShowPanel(this.GuestPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Guest Login Failed");
            }
        }

        private void RegisterButton_Click(object? sender, EventArgs e)
        {
            this.ShowPanel(this.RegisterPanel);

        }

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            string username = this.usernameTextBox.Text.Trim();
            string password = this.passwordTextBox.Text.Trim();

            try
            {
                // Authenticate the user
                User authenticatedUser = AuthenticationService.Login(username, password);
                MessageBox.Show($"Welcome {authenticatedUser.Name}!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Navigate to the appropriate panel
                if (authenticatedUser is Student)
                {
                    // greetings
                    this.Text += $" - Student";
                    this.StudentIdentifierLabel.Text += $"{authenticatedUser.Username}";

                    this.InitalizeAllStudentTabs();
                }
                else if (authenticatedUser is Donor)
                {
                    // Show the donor panel
                    //this.ShowPanel(this.DonorPanel);
                }
                else
                {
                    throw new Exception("Unkown User Type, Register or Continue as Guest.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitalizeAllStudentTabs()
        {
            this.HideAllViews();

            this.SwitchView(this.StudentView);

            this.InitializeStudentClubsTab();
            this.InitializeStudentProjectsTab();
            this.InitializeStudentScholarshipsTab();
            this.InitializeUserProfile();
        }

        private void SwitchView(TabControl userView)
        {
            userView.Parent = this;
            userView.Visible = true;
            userView.BringToFront();
        }

        /// <summary>
        /// This method initializes the guest dashboard.
        /// </summary>
        private void InitializeGuestDashboard()
        {
            // Load projects from the database and populate the projects combo box
            this.LoadProjects();
            this.PopulateProjectsComboBox();

            // Load scholarships from the database and populate the scholarships combo box
            this.LoadScholarships();
            this.PopulateScholarshipsComboBox();
        }

        private void InitializeStudentClubsTab()
        {
            // Hide the group boxes
            this.StudentClubsTab_AddClubForm_GroupBox.Visible = false;
            this.StudentClubsTab_RemoveClubForm_GroupBox.Visible = false;
            this.StudentClubsTab_JoinClubForm_GroupBox.Visible = false;

            // clear all the add club form fields
            this.StudentClubsTab_AddClubForm_Name_TextBox.Clear();
            this.StudentClubsTab_AddClubForm_Description_TextBox.Clear();

            // clear the remove club combo box
            this.StudentClubsTab_RemoveClub_ComboBox.Items.Clear();
            this.StudentClubsTab_JoinClub_ComboBox.Items.Clear();

            // Load clubs into the club list combo box
            this.LoadClubs();

            // set default states
            this.StudentClubsTab_ClubList_ComboBox.SelectedIndex = -1;
            this.StudentClubsTab_RemoveClub_ComboBox.SelectedIndex = -1;
            this.StudentClubsTab_JoinClub_ComboBox.SelectedIndex = -1;

            // Load the clubs that the student is in
            this.LoadAStudentsClubList();

            this.StudentClubsTab_MyClubs_ListBox.SelectedIndexChanged += this.StudentClubsTab_MyClubs_ListBox_SelectedIndexChanged;
        }

        private void InitializeStudentProjectsTab()
        {
            // Hide the group boxes
            this.StudentProjectsTab_RegisterProject_GroupBox.Visible = false;
            this.StudentProjectsTab_ViewProject_GroupBox.Visible = false;
            this.StudentProjectsTab_JoinProject_GroupBox.Visible = false;

            // clear the list box Items
            this.StudentProjectsTab_AllProjects_ComboBox.Items.Clear();

            // Load projects from the database
            this.LoadStudentProjects();

            // Load the clubs into the combo box
            this.LoadProjectsIntoComboBox(this.StudentProjectsTab_AllProjects_ComboBox);
            this.LoadProjectsIntoComboBox(this.StudentProjectsTab_JoinProject_ComboBox);

            // Set default states
            this.StudentProjectsTab_AllProjects_ComboBox.SelectedIndex = -1;

            // Attach event handlers
            this.StudentProjectsTab_AllProjects_ComboBox.SelectedIndexChanged += this.StudentProjectsTab_AllProjects_ComboBox_SelectedIndexChanged;
            this.StudentProjectsTab_ViewProject_ListBox.SelectedIndexChanged += this.StudentProjectsTab_ViewProject_ListBox_SelectedIndexChanged;
        }

        private void LoadClubsIntoComboBox(ComboBox comboBox)
        {
            // clear the combo box
            comboBox.Items.Clear();

            // Load clubs from the database
            List<Club> clubs = XMLDatabase.LoadClubs();

            // check if there are any clubs
            if (clubs.Count == 0)
            {
                comboBox.Items.Add("No clubs available");
                comboBox.SelectedIndex = 0;
                return;
            }

            // populate the combo box with club names
            foreach (var club in clubs)
            {
                comboBox.Items.Add(club);
            }
        }

        private void LoadProjectsIntoComboBox(ComboBox comboBox)
        {
            // clear the combo box
            comboBox.Items.Clear();

            // Load clubs from the database
            List<Project> projects = XMLDatabase.LoadProjects();

            // check if there are any clubs
            if (projects.Count == 0)
            {
                comboBox.Items.Add("No projects available");
                comboBox.SelectedIndex = 0;
                return;
            }

            // populate the combo box with club names
            foreach (var project in projects)
            {
                comboBox.Items.Add(project);
            }
        }

        private void LoadStudentProjects()
        {
            // Get the current student
            Student student = AuthenticationService.GetCurrentUser() as Student;

            // clear the list box to avoid duplicates
            this.StudentProjectsTab_ViewProject_ListBox.Items.Clear();

            if (student == null)
            {
                MessageBox.Show("Student not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Load projects from the database
            List<Project> projects = XMLDatabase.LoadProjects();

            // filter the projects that the student is in
            var studentProjects = projects.Where(p => p.Members.Contains(student));

            // populate the list box with the student's projects
            foreach (var project in studentProjects)
            {
                this.StudentProjectsTab_ViewProject_ListBox.Items.Add(project.Name);
            }

            // add a placeholder if the student is not in any projects
            if (this.StudentProjectsTab_ViewProject_ListBox.Items.Count == 0)
            {
                this.StudentProjectsTab_ViewProject_ListBox.Items.Add("You are not in any projects");
            }
        }

        private void StudentProjectsTab_AllProjects_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected club name
            string selectedProjectName = this.StudentProjectsTab_AllProjects_ComboBox.SelectedItem.ToString();

            if (selectedProjectName == "No project available")
            {
                return; // no club selected
            }

            // Load clubs from the database
            List<Project> projects = XMLDatabase.LoadProjects();

            // Find the selected club
            Project selectedProject = projects.FirstOrDefault(c => c.Name == selectedProjectName);

            if (selectedProject != null)
            {
                // Display project details
                MessageBox.Show(
                    $"Project Name: {selectedProject.Name}\n" +
                    $"Description: {selectedProject.Description}\n" +
                    $"Target Amount: {selectedProject.TargetAmount:C}\n" +
                    $"Raised Amount: {selectedProject.RaisedAmount:C}\n" +
                    $"Start Date: {selectedProject.StartDate}\n" +
                    $"End Date: {selectedProject.EndDate}\n",
                    "Project Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void StudentProjectsTab_ViewProject_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected project name
            string selectedProjectName = this.StudentProjectsTab_ViewProject_ListBox.SelectedItem.ToString();

            if (selectedProjectName == "You are not in any projects")
            {
                return; // no project selected
            }

            // Load projects from the database
            List<Project> projects = XMLDatabase.LoadProjects();

            // Find the selected project
            Project selectedProject = projects.FirstOrDefault(p => p.Name == selectedProjectName);

            if (selectedProject != null)
            {
                // Display project details
                MessageBox.Show(
                    $"Project Name: {selectedProject.Name}\n" +
                    $"Description: {selectedProject.Description}\n" +
                    $"Target Amount: {selectedProject.TargetAmount:C}\n" +
                    $"Raised Amount: {selectedProject.RaisedAmount:C}\n" +
                    $"Start Date: {selectedProject.StartDate}\n" +
                    $"End Date: {selectedProject.EndDate}\n",
                    "Project Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// This method populates the projects combo box.
        /// </summary>
        private void PopulateProjectsComboBox()
        {
            if (AuthenticationService.GetCurrentUser() is Guest)
            {
                this.GuestComboBox_Projects.Items.Clear();
                foreach (var project in this.projects)
                {
                    this.GuestComboBox_Projects.Items.Add(project.Name);
                }
            }

            if (AuthenticationService.GetCurrentUser() is Student)
            {
                throw new NotImplementedException();
            }

            if (AuthenticationService.GetCurrentUser() is Donor)
            {
                throw new NotImplementedException();
            }
        }

        private void PopulateScholarshipsComboBox()
        {
            if (AuthenticationService.GetCurrentUser() is Guest)
            {
                // Clear the combo box
                this.GuestComboBox_Projects.Items.Clear();

                // Load scholarships from the database
                List<Scholarship> scholarships_ = XMLDatabase.LoadScholarships();

                // Add scholarships to the combo box
                foreach (var scholarship in scholarships_)
                {
                    this.GuestComboBox_Scholarships.Items.Add(scholarship.Name);
                }

                // Store the scholarships for later use
                this.scholarships = scholarships_;
            }

            if (AuthenticationService.GetCurrentUser() is Student)
            {
                throw new NotImplementedException();
            }

            if (AuthenticationService.GetCurrentUser() is Donor)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This method shows the details of the selected project.
        /// </summary>
        /// <param name="sender">This represents the Item being modified.</param>
        /// <param name="e">Represents the details of the item being modified.</param>
        private void GuestComboBox_Projects_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (this.GuestComboBox_Projects.SelectedItem != null)
            {
                string selectedProjectName = this.GuestComboBox_Projects.SelectedItem.ToString();
                var selectedProject = this.projects.FirstOrDefault(p => p.Name == selectedProjectName); // find the project by name

                if (selectedProject != null)
                {
                    MessageBox.Show(
                        $"Project Name: {selectedProject.Name}\n" +
                        $"Description: {selectedProject.Description}\n" +
                        $"Target Amount: {selectedProject.TargetAmount:C}\n" +
                        $"Raised Amount: {selectedProject.RaisedAmount:C}\n"
                        + $"Start Date: {selectedProject.StartDate}\n" +
                        $"End Date: {selectedProject.EndDate}\n",
                        "Project Details",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// This method shows the details of the selected scholarship.
        /// </summary>
        /// <param name="sender">This represents the Item being modified.</param>
        /// <param name="e">Represents the details of the item being modified.</param>
        private void GuestComboBox_Scholarships_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.GuestComboBox_Scholarships.SelectedItem != null)
            {
                // Find the selected scholarship
                string selectedName = this.GuestComboBox_Scholarships.SelectedItem.ToString();
                var selectedScholarship = this.scholarships.FirstOrDefault(s => s.Name == selectedName);

                if (selectedScholarship != null)
                {
                    // Display scholarship details
                    MessageBox.Show(
                        $"Scholarship Name: {selectedScholarship.Name}\n" +
                        $"Description: {selectedScholarship.Description}\n" +
                        $"Status: {selectedScholarship.Status}\n" +
                        $"Criteria: {string.Join(", ", selectedScholarship.Criteria)}\n",
                        "Scholarship Details",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void GuestButton_Register_Click(object sender, EventArgs e)
        {
            // Show a registration panel or form for user input
            // If you have a separate RegisterPanel, switch to it
            this.ShowPanel(this.RegisterPanel);
        }

        private void GuestButton_Login_Click(object sender, EventArgs e)
        {
            this.GuestButton_Back_Click(sender, e); // go back to the previous panel
        }

        /// <summary>
        /// This method hids the current panel, which should be the Guest panel as it can only be accessed through here.
        /// </summary>
        /// <param name="sender">This represents the Item being modified.</param>
        /// <param name="e">Represents the details of the item being modified.</param>
        private void GuestButton_Back_Click(object sender, EventArgs e)
        {
            this.GoBack();
        }

        /// <summary>
        /// This method is responsible for backtracking between panels for the application.
        /// </summary>
        private void GoBack()
        {
            if (this.navigationStack.Count == 1) // last thing before the home page, which is not a panel.
            {
                this.navigationStack.Pop();
                this.currentPanel.Hide();
                this.currentPanel = null;

            }
            else if (this.navigationStack.Count > 1) // has visited other panels prior
            {
                this.currentPanel.Visible = false;

                Panel previousPanel = this.navigationStack.Pop();
                previousPanel = this.navigationStack.Peek();
                previousPanel.Visible = true;
                previousPanel.BringToFront();

                this.currentPanel = previousPanel;
            }
            else
            {
                MessageBox.Show("No previous panel to navigate to.", "Navigation", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// This method handles the identification of what user type is being created and adjusts the UI to the option.
        /// </summary>
        /// <param name="sender">This represents the Item being modified.</param>
        /// <param name="e">Represents the details of the item being modified.</param>
        private void RegisterRadioButton_Student_CheckedChanged(object sender, EventArgs e)
        {
            bool isStudent = this.RegisterRadioButton_Student.Checked;

            // show student-specific fields
            this.RegisterLabel_Major.Visible = isStudent;
            this.RegisterTextBox_Major.Visible = isStudent;
            this.RegisterLabel_WsuId.Visible = isStudent;
            this.RegisterTextBox_WsuId.Visible = isStudent;
            this.RegisterLabel_GraduationYear.Visible = isStudent;
            this.RegisterTextBox_GraduationYear.Visible = isStudent;

            // Hide donor-specific fields
            this.RegisterLabel_DonorId.Visible = !isStudent;
            this.RegisterTextBox_DonorId.Visible = !isStudent;
        }

        /// <summary>
        /// This method handles the identification of what user type is being created and adjusts the UI to the option.
        /// </summary>
        /// <param name="sender">This represents the Item being modified.</param>
        /// <param name="e">Represents the details of the item being modified.</param>
        private void RegisterRadioButton_Donor_CheckedChanged(object sender, EventArgs e)
        {
            // No need to implement this method since the student checked changed event handler already handles this.
        }

        private void RegisterButton_Submit_Click(object sender, EventArgs e)
        {
            // Collect common fields
            string name = this.RegisterTextBox_Name.Text;
            string email = this.RegisterTextBox_Email.Text;
            string username = this.RegisterTextBox_Username.Text;
            string password = this.RegisterTextBox_Password.Text;

            // validate common fields
            if (string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                User newUser;

                // check which account type is being created
                if (this.RegisterRadioButton_Student.Checked)
                {
                    // collect and validate student-specific fields
                    string major = this.RegisterTextBox_Major.Text;
                    string wsuId = this.RegisterTextBox_WsuId.Text;
                    string graduationYear = this.RegisterTextBox_GraduationYear.Text;

                    if (string.IsNullOrEmpty(major)
                        || string.IsNullOrEmpty(wsuId)
                        || string.IsNullOrEmpty(graduationYear))
                    {
                        MessageBox.Show("Please fill in all fields", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // create the student
                    newUser = UserFactory.CreateUser("student", name, email, username, password, $"Major: {major},WSU ID: {wsuId},Graduation Year: {graduationYear}");
                }
                else if (this.RegisterRadioButton_Donor.Checked)
                {
                    // collect and validate donor-specific fields
                    string donorId = this.RegisterTextBox_DonorId.Text;

                    if (string.IsNullOrEmpty(donorId))
                    {
                        MessageBox.Show("Please fill in all fields", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // create the donor
                    newUser = UserFactory.CreateUser("donor", name, email, username, password, $"Donor ID: {donorId}");
                }
                else
                {
                    MessageBox.Show("Please select an account type", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // register the user in the system.
                AuthenticationService.Register(newUser);

                // Notify the user
                MessageBox.Show($" {username} has been registered successfully!", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ClearRegistrationForm();

                // route to the dashboard
                // this.ShowPanel(this.StudentPanel);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegisterButton_Back_Click(object sender, EventArgs e)
        {
            this.GoBack();
        }

        private void ClearRegistrationForm()
        {
            this.RegisterTextBox_Name.Clear();
            this.RegisterTextBox_Email.Clear();
            this.RegisterTextBox_Username.Clear();
            this.RegisterTextBox_Password.Clear();
            this.RegisterTextBox_Major.Clear();
            this.RegisterTextBox_WsuId.Clear();
            this.RegisterTextBox_GraduationYear.Clear();
            this.RegisterTextBox_DonorId.Clear();
            this.RegisterRadioButton_Donor.Checked = false;
            this.RegisterRadioButton_Student.Checked = false;
        }

        private void LoadClubs()
        {
            List<Club> clubs = XMLDatabase.LoadClubs();
            this.StudentClubsTab_ClubList_ComboBox.Items.Clear();

            // Add clubs to the combo box
            foreach (var club in clubs)
            {
                this.StudentClubsTab_ClubList_ComboBox.Items.Add(club);
            }

            // Attaching the event handler for displaying club details
            this.StudentClubsTab_ClubList_ComboBox.SelectedIndexChanged += this.StudentClubsTab_ClubList_ComboBox_SelectedIndexChanged;
        }

        private void StudentClubsTab_ClubList_ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (this.StudentClubsTab_ClubList_ComboBox.SelectedItem is Club selectedClub)
            {
                // add start date and end date to the club details later on
                MessageBox.Show(
                    $"Club Name: {selectedClub.Name}\n" +
                    $"Description: {selectedClub.Description}\n" +
                    $"Start Date: {selectedClub.StartDate}\n" +
                    $"End Date: {selectedClub.EndDate}\n" +
                    $"Members: {string.Join(", ", selectedClub.Members.Select(m => m.Name))}\n" +
                    $"Projects: {string.Join(", ", selectedClub.Projects.Select(p => p.Name))}\n",
                    "Club Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void StudentClubsTab_AddClub_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.StudentClubsTab_AddClubForm_GroupBox.Visible = true;
        }

        private void StudentClubsTab_AddClubForm_Submit_Button_Click(object sender, EventArgs e)
        {
            // Get user inputs
            string clubName = this.StudentClubsTab_AddClubForm_Name_TextBox.Text;
            string clubDescription = this.StudentClubsTab_AddClubForm_Description_TextBox.Text;
            DateTime startDate = this.StudentClubsTab_AddClubForm_StartDate_DateTimePicker.Value;
            DateTime endDate = this.StudentClubsTab_AddClubForm_EndDate_DateTimePicker.Value;

            // Validate user inputs
            if (string.IsNullOrWhiteSpace(clubName) || string.IsNullOrWhiteSpace(clubDescription))
            {
                MessageBox.Show("Please fill in all fields", "Add Club Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create the club
            Club newClub = new Club(clubName, clubDescription);
            newClub.StartDate = startDate;
            newClub.EndDate = endDate;
            List<Club> clubs = new List<Club>();
            clubs.Add(newClub);

            // Save the club to the database
            XMLDatabase.SaveClubs(clubs);

            // Refresh thr clubs dropdowm
            this.LoadClubs();

            // hide the group box
            this.StudentClubsTab_AddClubForm_GroupBox.Visible = false;

            // clear the group box
            this.StudentClubsTab_AddClubForm_Name_TextBox.Clear();
            this.StudentClubsTab_AddClubForm_Description_TextBox.Clear();

            // Notify the user
            MessageBox.Show($"Club {clubName} has been added successfully!", "Add Club Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.LoadAStudentsClubList();
        }

        private void StudentClubsTab_RemoveClub_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Populate the combo box with clubs
            this.StudentClubsTab_RemoveClubForm_GroupBox.Visible = true;
            this.StudentClubsTab_RemoveClub_ComboBox.Items.Clear();
            foreach (var club in XMLDatabase.LoadClubs())
            {
                this.StudentClubsTab_RemoveClub_ComboBox.Items.Add(club.Name);
            }

            this.StudentClubsTab_RemoveClub_ComboBox.Visible = true;
            this.StudentClubsTab_ConfirmRemoveClub_Button.Visible = true;
        }

        private void StudentClubsTab_ConfirmRemoveClub_Button_Click(object sender, EventArgs e)
        {
            if (this.StudentClubsTab_RemoveClub_ComboBox.SelectedItem is Club selectedClub)
            {
                var confirmResult = MessageBox.Show($"Are you sure you want to remove {selectedClub.Name}?", "Confirm Remove Club", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    // Remove the club from the database
                    List<Club> clubs = XMLDatabase.LoadClubs();
                    clubs.Remove(selectedClub);
                    XMLDatabase.SaveClubs(clubs);

                    // Refresh the clubs dropdowm
                    this.LoadClubs();

                    // Hide the group box
                    this.StudentClubsTab_RemoveClubForm_GroupBox.Visible = false;

                    // Notify the user
                    MessageBox.Show($"Club {selectedClub.Name} has been removed successfully!", "Remove Club Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a club to remove", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StudentClubsTab_JoinClub_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Populate the combo box with clubs
            this.StudentClubsTab_JoinClubForm_GroupBox.Visible = true;
            this.StudentClubsTab_JoinClub_ComboBox.Items.Clear();
            foreach (var club in XMLDatabase.LoadClubs())
            {
                this.StudentClubsTab_JoinClub_ComboBox.Items.Add(club.Name);
            }

            this.StudentClubsTab_JoinClub_ComboBox.Visible = true;
            this.StudentClubsTab_JoinClub_Button.Visible = true;
        }

        private void StudentClubsTab_JoinClub_Button_Click(object sender, EventArgs e)
        {
            // Ensure a club is selected
            if (this.StudentClubsTab_JoinClub_ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a club to join", "Join Club Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the selected club name
            string clubName = this.StudentClubsTab_JoinClub_ComboBox.SelectedItem.ToString();

            // load the clubs from the database
            List<Club> clubs = XMLDatabase.LoadClubs();

            // find the selected club
            Club selectedClub = clubs.FirstOrDefault(c => c.Name == clubName);

            if (selectedClub == null)
            {
                MessageBox.Show("Club not found", "Join Club Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the current user
            Student currentStudent = AuthenticationService.GetCurrentUser() as Student;

            if (!selectedClub.Members.Contains(currentStudent))
            {
                selectedClub.AddMember(currentStudent);
                XMLDatabase.SaveClubs(clubs);
                MessageBox.Show($"You have joined {selectedClub.Name}", "Join Club Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("You are already a member of this club", "Join Club Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.LoadClubs();
        }

        private void LoadAStudentsClubList()
        {
            // clear the list box
            this.StudentClubsTab_MyClubs_ListBox.Items.Clear();

            // get the current student
            Student student = AuthenticationService.GetCurrentUser() as Student;

            // Load the clubs from the database
            List<Club> clubs = XMLDatabase.LoadClubs();

            // Find the clubs that the student is in
            foreach (var club in clubs)
            {
                if (club.Members.Contains(student))
                {
                    this.StudentClubsTab_MyClubs_ListBox.Items.Add(club.Name);
                }
            }

            if (this.StudentClubsTab_MyClubs_ListBox.Items.Count == 0)
            {
                this.StudentClubsTab_MyClubs_ListBox.Items.Add("You are not in any clubs");
            }
        }

        private void StudentClubsTab_MyClubs_ListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // get the selected club name
            string selectedClubName = this.StudentClubsTab_MyClubs_ListBox.SelectedItem.ToString();

            if (selectedClubName == "You are not in any clubs" || string.IsNullOrEmpty(selectedClubName))
            {
                return; // no club selected
            }

            // load the clubs from the database
            List<Club> clubs = XMLDatabase.LoadClubs();

            // find the selected club
            Club selectedClub = clubs.FirstOrDefault(c => c.Name == selectedClubName);

            if (selectedClub != null)
            {
                // Display club details
                MessageBox.Show(
                    $"Club Name: {selectedClub.Name}\n" +
                    $"Description: {selectedClub.Description}\n" +
                    $"Start Date: {selectedClub.StartDate}\n" +
                    $"End Date: {selectedClub.EndDate}\n" +
                    $"Members: {string.Join(", ", selectedClub.Members.Select(m => m.Name))}\n" +
                    $"Projects: {string.Join(", ", selectedClub.Projects.Select(p => p.Name))}\n",
                    "Club Details",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void StudentProjectsTab_ViewProject_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            // Check if a project is selected in the combo box
            if (this.StudentProjectsTab_AllProjects_ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("No project has been selected. Please select a project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If a project is selected, display its details
            string selectedProject = this.StudentProjectsTab_AllProjects_ComboBox.SelectedItem.ToString();
            MessageBox.Show($"Details for project: {selectedProject}", "Project Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StudentProjectsTab_RegisterProject_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.StudentProjectsTab_RegisterProject_GroupBox.Visible = true;
            this.LoadClubsIntoComboBox(this.StudentProjectsTab_ProjectClub_ComboBox);
        }

        private void StudentProjectsTab_ViewProjects_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Show the View My Projects group box
            this.StudentProjectsTab_ViewProject_GroupBox.Visible = true;

            // Optionally, you can refresh the list box with the student's projects
            this.LoadStudentProjects();
        }

        private void StudentProjectsTab_ProjectSubmit_Button_Click(object sender, EventArgs e)
        {
            // Get the user inputs
            string projectName = this.StudentProjectsTab_ProjectName_TextBox.Text;
            string projectDescription = this.StudentProjectsTab_ProjectDescription_TextBox.Text;
            string projectTargetAmount = this.StudentProjectsTab_ProjectDescription_TextBox.Text;
            string targetAmountText = this.StudentProjectsTab_TargetAmount_TextBox.Text;
            DateTime startDate = this.StudentProjectsTab_StartDate_DateTimePicker.Value;
            DateTime endDate = this.StudentProjectsTab_EndDate_DateTimePicker.Value;
            Club selectedClub = this.StudentProjectsTab_ProjectClub_ComboBox.SelectedItem as Club;

            // Validate the user inputs
            if (string.IsNullOrWhiteSpace(projectName)
                || string.IsNullOrWhiteSpace(projectDescription)
                || string.IsNullOrWhiteSpace(targetAmountText)
                || selectedClub == null)
            {
                MessageBox.Show("Please fill in all fields", "Register Project Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Parse the target amount
            if (!decimal.TryParse(targetAmountText, out decimal targetAmount))
            {
                MessageBox.Show("Please enter a valid target amount", "Register Project Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (startDate >= endDate)
            {
                MessageBox.Show("Start date must be before end date", "Register Project Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create the project
            Project newProject = new Project(projectName, projectDescription, targetAmount);
            newProject.StartDate = startDate;
            newProject.EndDate = endDate;
            newProject.UpdateClub(selectedClub);

            // Save the project to the database
            List<Project> projects = new List<Project>();
            projects.Add(newProject);
            XMLDatabase.SaveProjects(projects);

            // clear the form
            this.StudentProjectsTab_ProjectName_TextBox.Clear();
            this.StudentProjectsTab_ProjectDescription_TextBox.Clear();
            this.StudentProjectsTab_TargetAmount_TextBox.Clear();
            this.StudentProjectsTab_ProjectClub_ComboBox.SelectedIndex = -1;
            this.StudentProjectsTab_StartDate_DateTimePicker.Value = DateTime.Now;
            this.StudentProjectsTab_EndDate_DateTimePicker.Value = DateTime.Now;

            // Notify the user
            MessageBox.Show($"Project {projectName} has been registered successfully!", "Register Project Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Refresh the projects dropdowm
            this.LoadStudentProjects();
            this.LoadProjectsIntoComboBox(this.StudentProjectsTab_AllProjects_ComboBox);
            this.LoadProjectsIntoComboBox(this.StudentProjectsTab_JoinProject_ComboBox);
        }

        private void StudentProjectsTab_ProjectCancel_Button_Click(object sender, EventArgs e)
        {
            // clear all the fields
            this.StudentProjectsTab_ProjectName_TextBox.Clear();
            this.StudentProjectsTab_ProjectDescription_TextBox.Clear();
            this.StudentProjectsTab_TargetAmount_TextBox.Clear();
            this.StudentProjectsTab_ProjectClub_ComboBox.SelectedIndex = -1;
            this.StudentProjectsTab_StartDate_DateTimePicker.Value = DateTime.Now;
            this.StudentProjectsTab_EndDate_DateTimePicker.Value = DateTime.Now;
        }

        private void StudentProjectsTab_ProjectJoin_Button_Click(object sender, EventArgs e)
        {
            // get the selected project name from the dropdown
            Project selectedProject = this.StudentProjectsTab_JoinProject_ComboBox.SelectedItem as Project;

            // validate the selected project
            if (selectedProject == null)
            {
                MessageBox.Show("Please select a project to join", "Join Project Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // get the current student
            Student currentStudent = AuthenticationService.GetCurrentUser() as Student;

            // check if the student is already in the project
            if (!selectedProject.Members.Contains(currentStudent))
            {
                selectedProject.AddMember(currentStudent);
                XMLDatabase.SaveProjects(new List<Project> { selectedProject });
                MessageBox.Show($"You have joined {selectedProject.Name}", "Join Project Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("You are already a member of this project", "Join Project Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // refresh the projects list
            this.LoadStudentProjects();

        }

        private void StudentProjectsTab_JoinProject_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.StudentProjectsTab_JoinProject_GroupBox.Visible = true;
        }

        private void StudentScholarshipTab_ViewScholarship_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Check if a project is selected in the combo box
            if (this.StudentScholarshipTab_ScholarshipList_ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("No scholarship has been selected. Please select a scholarship.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If a project is selected, display its details
            Scholarship selectedScholarship = this.StudentScholarshipTab_ScholarshipList_ComboBox.SelectedItem as Scholarship;
            MessageBox.Show($"Details for Scholarship:\n\n" +
                $" Scholarship Name: {selectedScholarship.Name}\n\n" +
                $"Scholarship Description: {selectedScholarship.Description}\n",
                "Project Details", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void StudentScholarshipTab_ApplyForScholarship_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.StudentScholarshipTab_ApplyToScholarship_GroupBox.Visible = true;
        }

        private void StudentScholarshipTab_ViewApplications_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.StudentScholarshipTab_ViewApplications_GroupBox.Visible = true;
            this.AwardMyselfScholarshipButton.Visible = true;
        }

        private void StudentScholarshipTab_ApplyScholarship_Submit_Button_Click(object sender, EventArgs e)
        {
            // grab user inputs
            string scholarshipName = this.StudentScholarshipTab_Name_TextBox.Text;
            string scholarshipDescription = this.StudentScholarshipTab_Description_TextBox.Text;
            string scholarshipStatus = this.StudentScholarshipTab_ApplyScholarship_Status_TextBox.Text;
            string requirements = this.StudentScholarshipTab_ApplyScholarship_Requirements_TextBox.Text;

            // validate user inputs
            if (string.IsNullOrWhiteSpace(scholarshipName)
                || string.IsNullOrWhiteSpace(scholarshipDescription)
                || string.IsNullOrWhiteSpace(scholarshipStatus)
                || string.IsNullOrWhiteSpace(requirements))
            {
                MessageBox.Show("Please fill in all fields", "Apply Scholarship Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> criterion = requirements.Split(',').ToList();

            // create the scholarship
            Scholarship newScholarship = new Scholarship(scholarshipName, scholarshipDescription, scholarshipStatus, criterion);
            newScholarship.Applicants.Add(AuthenticationService.GetCurrentUser() as Student);

            // save to the database
            List<Scholarship> scholarships = new List<Scholarship>();
            scholarships.Add(newScholarship);
            XMLDatabase.SaveScholarships(scholarships);

            MessageBox.Show($"Successfully applied for the scholarship: {scholarshipName}", "Application Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // clear the form
            this.StudentScholarshipTab_ApplyScholarship_Cancel_Button_Click(sender, e);

            // refresh the scholarships list
            this.LoadStudentScholarships();

        }

        private void StudentScholarshipTab_ApplyScholarship_Cancel_Button_Click(object sender, EventArgs e)
        {
            this.StudentScholarshipTab_Name_TextBox.Clear();
            this.StudentScholarshipTab_Description_TextBox.Clear();
            this.StudentScholarshipTab_ApplyScholarship_Status_TextBox.Clear();
            this.StudentScholarshipTab_ApplyScholarship_Requirements_TextBox.Clear();
        }

        private void LoadStudentScholarships()
        {
            this.StudentScholarshipTab_ScholarshipList_ComboBox.Items.Clear(); // clear existing items
            List<Scholarship> scholarships = new List<Scholarship>();
            scholarships = XMLDatabase.LoadScholarships();

            foreach (var scholarship in scholarships)
            {
                this.StudentScholarshipTab_ScholarshipList_ComboBox.Items.Add(scholarship);
            }

            //if (scholarships.Count == 0)
            //{
            //    MessageBox.Show("No scholarships available at the moment.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void LoadStudentScholarshipApplications()
        {
            Student currentStudent = AuthenticationService.GetCurrentUser() as Student;

            if (currentStudent == null)
            {
                MessageBox.Show("Unable to load your applications.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Scholarship> scholarships = XMLDatabase.LoadScholarships() as List<Scholarship>;
            this.StudentScholarship_ViewApplications_DataGridTable.Rows.Clear();

            foreach (var scholarship in scholarships)
            {
                // if the student is an applicant
                if (scholarship.Applicants.Contains(currentStudent))
                {
                    string decision = scholarship.AwardedStudents.Contains(currentStudent) ? "Awarded" : "Pending";

                    // add a row to the datagrid
                    this.StudentScholarship_ViewApplications_DataGridTable.Rows.Add(
                            scholarship.Name,
                            scholarship.Status,
                            decision
                        );
                }
                else
                {
                    this.StudentScholarship_ViewApplications_DataGridTable.Rows.Add(
                            scholarship.Name,
                            scholarship.Status,
                            "N/A"
                        );
                }
            }
        }

        private void InitializeStudentScholarshipsTab()
        {
            // hide all group boxes
            this.StudentScholarshipTab_ApplyToScholarship_GroupBox.Visible = false;
            this.StudentScholarshipTab_ViewApplications_GroupBox.Visible = false;
            this.AwardMyselfScholarshipButton.Visible = false;

            // clear all the fields
            this.StudentScholarshipTab_Name_TextBox.Clear();
            this.StudentScholarshipTab_Description_TextBox.Clear();
            this.StudentScholarshipTab_ApplyScholarship_Status_TextBox.Clear();
            this.StudentScholarshipTab_ApplyScholarship_Requirements_TextBox.Clear();

            // load scholarships
            this.LoadStudentScholarships();

            // load scholarship applications
            this.LoadStudentScholarshipApplications();

            // set default states
            this.StudentScholarshipTab_ScholarshipList_ComboBox.SelectedIndex = -1;

            // attach event handlers
            this.StudentScholarshipTab_ViewApplications_LinkLabel.LinkClicked += this.StudentScholarshipTab_ViewApplications_LinkLabel_LinkClicked;
            this.StudentScholarshipTab_ApplyForScholarship_LinkLabel.LinkClicked += this.StudentScholarshipTab_ApplyForScholarship_LinkLabel_LinkClicked;
            this.StudentScholarshipTab_ViewScholarship_LinkLabel.LinkClicked += this.StudentScholarshipTab_ViewScholarship_LinkLabel_LinkClicked;
            this.StudentScholarshipTab_ApplyScholarship_Submit_Button.Click += this.StudentScholarshipTab_ApplyScholarship_Submit_Button_Click;
            this.StudentScholarshipTab_ApplyScholarship_Cancel_Button.Click += this.StudentScholarshipTab_ApplyScholarship_Cancel_Button_Click;
        }

        private void AwardMyselfScholarshipButton_Click(object sender, EventArgs e)
        {
            Student currentUser = AuthenticationService.GetCurrentUser() as Student;
            if (currentUser == null)
            {
                MessageBox.Show("Unable to award scholarship to yourself.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // get the selected scholarship
            Scholarship selectedScholarship = this.StudentScholarshipTab_ScholarshipList_ComboBox.SelectedItem as Scholarship;
            if (selectedScholarship == null)
            {
                MessageBox.Show("Please select a scholarship to award to yourself.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check if the student has already been awarded the scholarship
            if (selectedScholarship.AwardedStudents.Contains(currentUser))
            {
                MessageBox.Show("You have already been awarded this scholarship.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // award the scholarship to the student
            selectedScholarship.AwardedStudents.Add(currentUser);

            // save the scholarship to the database
            XMLDatabase.SaveScholarships(new List<Scholarship> { selectedScholarship });

            // Update the UI
            MessageBox.Show("Congratulations! You have awarded yourself the scholarship!", "CHEAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.LoadStudentScholarshipApplications();
        }

        private void UploadProfilePicture_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp",
                Title = "Select Profile Picture",
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;

                // Display the selected image in the picture box
                this.UploadProfilePicture.Visible = false;
                this.ProfilePictureBox.Image = Image.FromFile(selectedFile);

                // Save the image to the database
                this.SaveProfilePicture(selectedFile);

                this.UpdateProfilePicture.Visible = true;
            }
        }

        private void InitializeUserProfile()
        {
            this.LoadProfilePicture();

            this.LoadUserProfileDetails();

            this.EditUserProfileDetails_GroupBox.Visible = false;
            this.DisplayUserProfileDetails_GroupBox.Visible = true;

            if (this.ProfilePictureBox.Image == null)
            {
                this.UpdateProfilePicture.Visible = false;
                this.UploadProfilePicture.Visible = true;
            }
            else
            {
                this.UploadProfilePicture.Visible = false;
                this.UpdateProfilePicture.Visible = true;
            }
        }

        private void LoadUserProfileDetails()
        {
            if (AuthenticationService.GetCurrentUser() is Student)
            {
                Student student = AuthenticationService.GetCurrentUser() as Student;
                this.DisplayUserProfileDetails_Name_Value.Text = student.Name;
                this.DisplayUserProfileDetails_Email_Value.Text = student.Email;
                this.DisplayUserProfileDetails_Username_Value.Text = student.Username;
                this.DisplayUserProfileDetails_WsuID_Value.Text = student.WsuId;
                this.DisplayUserProfileDetails_Major_Value.Text = student.Major;
                this.DisplayUserProfileDetails_Graduation_Value.Text = student.GraduationYear;
                this.DisplayUserProfileDetails_DonorId_Value.Visible = false;
                this.DisplayUserProfileDetails_DonorId_Label.Visible = false;
            }
            else if (AuthenticationService.GetCurrentUser() is Donor)
            {
                Donor donor = AuthenticationService.GetCurrentUser() as Donor;
                this.DisplayUserProfileDetails_Name_Value.Text = donor.Name;
                this.DisplayUserProfileDetails_Email_Value.Text = donor.Email;
                this.DisplayUserProfileDetails_Username_Value.Text = donor.Username;
                this.DisplayUserProfileDetails_DonorId_Value.Text = donor.DonorId;
            }
        }

        private void EditUserProfileDetails_Cancel_Button_Click(object sender, EventArgs e)
        {
            // hide the edit group box
            this.EditUserProfileDetails_GroupBox.Visible = false;

            // show the display group box
            this.DisplayUserProfileDetails_GroupBox.Visible = true;

        }

        private void EditUserProfileDetails_SaveChanges_Button_Click(object sender, EventArgs e)
        {
            // get the current user
            User currentUser = AuthenticationService.GetCurrentUser();

            // validate the user inputs
            if (currentUser is Student
                && string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Name_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Email_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Username_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_WsuId_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Major_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Graduation_Value.Text))
            {
                MessageBox.Show("Please fill in all fields", "Edit Profile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (currentUser is Donor
                && string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Name_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Email_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_Username_Value.Text)
                || string.IsNullOrWhiteSpace(this.EditUserProfileDetails_DonorId_Value.Text)
                )
            {
                MessageBox.Show("Please fill in all fields", "Edit Profile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // update the user's details
            currentUser.Name = this.EditUserProfileDetails_Name_Value.Text;
            currentUser.Email = this.EditUserProfileDetails_Email_Value.Text;
            currentUser.Username = this.EditUserProfileDetails_Username_Value.Text;

            if (currentUser is Student)
            {
                Student currentStudent = currentUser as Student;
                currentStudent.updateWsuId(this.EditUserProfileDetails_WsuId_Value.Text);
                currentStudent.Major = this.EditUserProfileDetails_Major_Value.Text;
                currentStudent.GraduationYear = this.EditUserProfileDetails_Graduation_Value.Text;
            }
            else if (currentUser is Donor)
            {
                Donor currentDonor = currentUser as Donor;
                currentDonor.updateDonorId(this.EditUserProfileDetails_DonorId_Value.Text);
            }

            // hide the edit group box
            this.EditUserProfileDetails_GroupBox.Visible = false;

            // refresh the display group box
            this.LoadUserProfileDetails();

            MessageBox.Show("Profile updated successfully!", "Edit Profile Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // show the display group box
            this.DisplayUserProfileDetails_GroupBox.Visible = true;
        }

        private void DisplayUserProfileDetails_EditProfile_Button_Click(object sender, EventArgs e)
        {
            // Hide the display group box
            this.DisplayUserProfileDetails_GroupBox.Visible = false;

            // Show the edit group box
            this.EditUserProfileDetails_GroupBox.Visible = true;

            // get the current user
            User currentUser = AuthenticationService.GetCurrentUser();

            // Populate the edit group box with the user's details
            this.EditUserProfileDetails_Name_Value.Text = currentUser.Name;
            this.EditUserProfileDetails_Email_Value.Text = currentUser.Email;
            this.EditUserProfileDetails_Username_Value.Text = currentUser.Username;
            if (currentUser is Student)
            {
                // hide donor-specific fields
                this.EditUserProfileDetails_DonorId_Label.Visible = false;
                this.EditUserProfileDetails_DonorId_Value.Visible = false;

                Student currentStudent = currentUser as Student;
                this.EditUserProfileDetails_WsuId_Value.Text = currentStudent.WsuId;
                this.EditUserProfileDetails_Major_Value.Text = currentStudent.Major;
                this.EditUserProfileDetails_Graduation_Value.Text = currentStudent.GraduationYear;
            }
            else if (currentUser is Donor)
            {
                Donor currentDonor = currentUser as Donor;
                this.EditUserProfileDetails_DonorId_Value.Text = currentDonor.DonorId;

                // hide student-specific fields
                this.EditUserProfileDetails_WsuId_Label.Visible = false;
                this.EditUserProfileDetails_WsuId_Value.Visible = false;
                this.EditUserProfileDetails_Major_Label.Visible = false;
                this.EditUserProfileDetails_Major_Value.Visible = false;
                this.EditUserProfileDetails_Graduation_Label.Visible = false;
                this.EditUserProfileDetails_Graduation_Value.Visible = false;

            }

        }

        private void ProfilePictureBox_Click(object sender, EventArgs e)
        {
            this.UploadProfilePicture_LinkClicked(sender, null);
        }

        private void SaveProfilePicture(string filePath)
        {
            // get the current user
            User currentUser = AuthenticationService.GetCurrentUser();

            string fileName = $"{currentUser.Username}_profile_picture.jpg";
            string pictureFilePath = Path.Combine(@"C:\\Users\\Ehiso\\OneDrive\\Desktop\\WSU CLASSES\\Senior_1st_Semester\\CPTS 321\\Coding\\inClassActivities\\cpts321-ehiane_oigiagbe-in-class-execises\\WSUFundraiserEngine\\", fileName);

            // ensure the file path exists
            string directory = Path.GetDirectoryName(pictureFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // copy the file to the destination
            File.Copy(filePath, pictureFilePath, true);

            // display a success message
            MessageBox.Show("Profile picture uploaded successfully!", "Profile Picture Upload", MessageBoxButtons.OK, MessageBoxIcon.Information);

            using (var imageStream = new FileStream(pictureFilePath, FileMode.Open, FileAccess.Read))
            {
                this.ProfilePictureBox.Image = Image.FromStream(imageStream);
            }

        }

        private void UpdateProfilePicture_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // open file dialog to select a new image
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp",
                Title = "Select Profile Picture",
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;

                // Display the selected image in the picture box
                using (var imageStream = new FileStream(selectedFile, FileMode.Open, FileAccess.Read))
                {
                    this.ProfilePictureBox.Image = Image.FromStream(imageStream);
                }

                // Save the image to the database
                this.SaveProfilePicture(selectedFile);
            }

            this.LoadProfilePicture();
        }

        private void LoadProfilePicture()
        {
            // get the current user
            User currentUser = AuthenticationService.GetCurrentUser();

            string fileName = $"{currentUser.Username}_profile_picture.jpg";
            string pictureFilePath = Path.Combine(@"C:\\Users\\Ehiso\\OneDrive\\Desktop\\WSU CLASSES\\Senior_1st_Semester\\CPTS 321\\Coding\\inClassActivities\\cpts321-ehiane_oigiagbe-in-class-execises\\WSUFundraiserEngine\\", fileName);

            if (File.Exists(pictureFilePath))
            {
                this.ProfilePictureBox.Image = Image.FromFile(pictureFilePath);
            }
            else
            {
                this.ProfilePictureBox.Image = null;
            }
        }

        // take these out later
        private void label4_Click(object sender, EventArgs e)
        { }

        private void label5_Click(object sender, EventArgs e)
        { }

        private void StudentClubsTab_AddClubForm_GroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void StudentClubs_Click(object sender, EventArgs e)
        {

        }

        private void StudentClubsTab_AddClubForm_StartDate_DateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

    }
}