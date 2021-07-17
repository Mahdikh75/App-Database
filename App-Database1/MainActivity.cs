using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using SQLite;

namespace App_Database1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnAdd, btnDel, btnSelect, btnUp, btnQA;
        TextView Result;
        EditText TName, TFamily, TAge;

        SQLiteConnection sqlite;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btnAdd = (Button)FindViewById(Resource.Id.button1);
            btnDel = (Button)FindViewById(Resource.Id.button2);
            btnUp = (Button)FindViewById(Resource.Id.button3);
            btnSelect = (Button)FindViewById(Resource.Id.button4);
            btnQA = (Button)FindViewById(Resource.Id.button5);

            TName = (EditText)FindViewById(Resource.Id.editText1);
            TFamily = (EditText)FindViewById(Resource.Id.editText2);
            TAge = (EditText)FindViewById(Resource.Id.editText3);

            Result = (TextView)FindViewById(Resource.Id.textView1);

            btnAdd.Click += BtnAdd_Click;
            btnDel.Click += BtnDel_Click;
            btnUp.Click += BtnUp_Click;
            btnSelect.Click += BtnSelect_Click;
            btnQA.Click += BtnQA_Click;

            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Test.db3");
            sqlite = new SQLiteConnection(path, true);
            sqlite.CreateTable<item_human>();

        }

        private void BtnQA_Click(object sender, System.EventArgs e)
        {
            Result.Text = "";

            var list_ages = sqlite.Query<item_human>("select * from item_human where Age >= 20", "?");
            foreach (var item in list_ages)
            {
                Result.Text += item.ID + " | " + item.Name + " | " + item.Family + " | " + item.Age + "\n";
            }

            //var px = sqlite.Table<item_human>().ToList();
            //foreach (var item in px)
            //{
            //    if (item.Age > 20)
            //    {
            //        Result.Text += item.ID + " | " + item.Name + " | " + item.Family + " | " + item.Age + "\n";
            //    }
            //}
        }

        private void BtnSelect_Click(object sender, System.EventArgs e)
        {
            ListData();
        }

        private void BtnUp_Click(object sender, System.EventArgs e)
        {
            if (TName.Text != "" && TFamily.Text != "" && TAge.Text != "")
            {
                int id = 3;
                sqlite.Update(new item_human() { ID = id, Name = TName.Text, Family = TFamily.Text, Age = int.Parse(TAge.Text) });
                ListData();
            }
        }

        private void BtnDel_Click(object sender, System.EventArgs e)
        {
            //sqlite.DeleteAll<item_human>();
            if (TAge.Text != "") // id 
            {
                sqlite.Delete<item_human>(int.Parse(TAge.Text));
                ListData();
            }
        }

        public void ListData()
        {
            // select 
            Result.Text = "";
            var list = sqlite.Query<item_human>("select * from item_human", "?");
            var all_list = sqlite.Table<item_human>().ToArray();

            foreach (var item in list)
            {
                Result.Text += item.ID + " | " + item.Name + " | " + item.Family + " | " + item.Age + "\n";
            }
        }

        private void BtnAdd_Click(object sender, System.EventArgs e)
        {
            if (TName.Text != "" && TFamily.Text != "" && TAge.Text != "")
            {
                sqlite.Insert(new item_human() { Name = TName.Text, Family = TFamily.Text, Age = int.Parse(TAge.Text) });
                TName.Text = ""; TFamily.Text = ""; TAge.Text = "";
                ListData();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


    [Table("item_human")]
    public class item_human
    {
        [PrimaryKey , AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }

        [MaxLength(3)]
        public int Age { get; set; }

    }

}