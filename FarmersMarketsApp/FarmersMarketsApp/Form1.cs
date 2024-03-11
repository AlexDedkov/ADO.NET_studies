using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace FarmersMarketsApp
{
    public partial class Form1 : Form
    {
        private farmersmarketsEntities dbContext;
        public Form1()
        {
            InitializeComponent();
            dbContext = new farmersmarketsEntities();
            PopulateTablesComboBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void PopulateTablesComboBox()
        {
            string[] tables = {"category", "market" };
            comboBox1.DataSource = tables;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {   
            string selectedTable = comboBox1.SelectedItem.ToString();
            DisplayTable(selectedTable);
        }

        private void DisplayTable(string tableName)
        {
            switch (tableName)
            {
                case "category":
                    var categories = dbContext.categories.ToList();
                    dataGridView1.DataSource = categories;
                    break;
                case "market":
                    var markets = dbContext.markets.ToList();
                    dataGridView1.DataSource = markets;
                    break;
                default:
                    MessageBox.Show("Invalid table name selected.");
                    break;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }
        private void SaveChanges()
        {
            try
            {
                dbContext.SaveChanges();
                MessageBox.Show("Changes saved successfully.");
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendFormat("Entity of type {0} has validation error \"{1}\" for property {2}.\n",
                            validationErrors.Entry.Entity.GetType().FullName,
                            validationError.ErrorMessage,
                            validationError.PropertyName);
                    }
                }

                MessageBox.Show("Error saving changes: " + ex.Message + "\nValidation Errors:\n" + sb.ToString());
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message + "\nInner Exception: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.EndEdit();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            dbContext.Dispose();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.SelectedItem.ToString();

            try
            {
                switch (selectedTable)
                {
                    case "category":
                        var newCategory = new category();
                        newCategory.category_id = GenerateUniqueCategoryId();
                        dbContext.categories.Add(newCategory);
                        break;
                    case "market":
                        var newMarket = new market();
                        newMarket.market_id = GenerateUniqueCategoryIdM();
                        dbContext.markets.Add(newMarket);
                        break;
                    default:
                        MessageBox.Show("Invalid table name selected.");
                        return;
                }

                // Save changes to the database
                SaveChanges();

                // Refresh DataGridView
                DisplayTable(selectedTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding new record: " + ex.ToString());
            }
        }

        // Method to generate a unique category_id
        private int GenerateUniqueCategoryId()
        {
            int maxCategoryId = dbContext.categories.Max(c => c.category_id);
            return maxCategoryId + 1;
        }

        private int GenerateUniqueCategoryIdM()
        {
            int maxMarketId = dbContext.markets.Max(c => c.market_id);
            return maxMarketId + 1;
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.SelectedItem.ToString();
            var dataGridView = dataGridView1;

            switch (selectedTable)
            {
                case "category":
                    foreach (DataGridViewRow row in dataGridView.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            var categoryToDelete = (category)row.DataBoundItem;
                            dbContext.Entry(categoryToDelete).State = EntityState.Deleted;
                        }
                    }
                    break;
                case "market":
                    foreach (DataGridViewRow row in dataGridView.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            var marketToDelete = (market)row.DataBoundItem;
                            dbContext.Entry(marketToDelete).State = EntityState.Deleted;
                        }
                    }
                    break;
                default:
                    MessageBox.Show("Invalid table name selected.");
                    return;
            }

            // Save changes to the database
            SaveChanges();

            // Refresh DataGridView
            DisplayTable(selectedTable);
        }
    }
}
