using System.Runtime.CompilerServices;

namespace market_flow_forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                (sender as Button).Enabled = false;

                var categories = new List<DTO.Mercado_Livre.MlCategory>();

                categories = await BLL.Mercado_Livre.Categories.GetCategories();

                var conn = DAL.Connection.Get();

                await DAL.Mercado_Livre.Categories.InsertCategoriesBatch(conn, categories);

                (sender as Button).Enabled = true;

                MessageBox.Show("Finalizou", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }
    }
}
