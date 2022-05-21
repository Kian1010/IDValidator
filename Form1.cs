using System.Globalization;
namespace ValidateID
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            richOutput.Clear();
            string userID = txtbxinput.Text;
            
            //ensures that the data being input is accurate, such as only containing numbers.
            if (CheckFormatting(userID) == false)
            {
                return;
            }

            //checks the ID using luhn algorithm to ensure it is valid/real
            if ( isRealID(userID) == false)
            {
                richOutput.Text = "ID typed incorrectly or this is an invalid ID\n";
                return;
            }
            else
            {
                richOutput.Text += "This ID is valid!\n";
            }

            //outputs data based on info extracted from ID 
            richOutput.Text += getDOB(userID)+ getGender(userID) + getCitizenship(userID);
           
        }

        private Boolean isRealID(string inputID)
        {
            string digits = "";
            int add;
            int sum = 0;
            int[] arrID = new int[13];

            //adds ID numbers into the array
            for (int i = 0; i < arrID.Length; i++)
            {
                arrID[i] = (int)char.GetNumericValue(inputID[i]);
            }

            //loops through the ID, using the luhn algorithm to create the values needed to ensure the ID is valid
            for (int i = arrID.Length-1; i > -1; i--)
            {
                if ( i % 2 == 1)
                {                    
                    arrID[i] = arrID[i] * 2;
                    if (arrID[i].ToString().Length == 2)
                    {
                        digits = arrID[i].ToString();
                        add = Int32.Parse(digits[0].ToString()) + Int32.Parse(digits[1].ToString());
                        arrID[i] = add;
                    }
                }
            }

            foreach (var item in arrID)
            {
                sum += item;
            }

            //if the value is divisible by 10 with no remainder the ID is valid
            if (sum % 10 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string getCitizenship(string inputID)
        {
            int citizenship = ( (int)char.GetNumericValue(inputID[10]) );
            
            // simply checking the 11th number of the ID is 0 or 1
            if ( citizenship == 0 )
            {
                return "You are an SA citizen\n";
            }
            else
            {
                return "You are a permanent resident";
            }
        }

        private string getGender(string inputID)
        {
            int genderID = ((int)char.GetNumericValue(inputID[6]));
                       
            //gets gender from 7th number in ID. if value is greater than 4 it is male, else female
            if (genderID > 4)
            {
                return "Your gender is Male\n";
            }
            else
            {
                return "Your gender is Female\n";
            }

        }
        private string getDOB(string inputID)
        {
            string year, month, day,output;
            int age;
            
            year = $"{inputID[0]}{inputID[1]}";
            month = $"{inputID[2]}{inputID[3]}";
            day = $"{inputID[4]}{inputID[5]}";

            //gets the correct year someone was born in, may be inaccurate if someone is older than 100 years
            if (System.Convert.ToInt32(year) > 22)
            {
                year = "19" + year;
            }
            else
            {
                year = "20" + year;
            }
            
            //gets the date the ID holder was born in, including the day 
            age = DateTime.Now.Year - System.Convert.ToInt32(year);
            string DateOfBirth = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day))
                                                                            .ToString("D", CultureInfo.InvariantCulture);
                      output = $"You were born on: {DateOfBirth}\nYour age this year is: {age}";
            return output;
        }



        private Boolean CheckFormatting(string inputID)
        {
            //simple checks to ensure the user inputs actual numbers correctly
            if (inputID.Length != 13)
            {
                MessageBox.Show("The length of your ID is incorrect!");
                return false;
            }
            else if (!inputID.All(Char.IsDigit))
            {
                MessageBox.Show("Please only input digits");
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}