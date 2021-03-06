﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Xml;

namespace stationary_cost_form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_click(object sender, EventArgs e)
        {
            int quantity;
            decimal priceEach, extendedPrice, subTotal, taxRate, salesTax, grandTotal;


            // Validate the row, if it fails then return
            subTotal = 0;

            if (ValidateRow(description1, quantity1, price1, 
                out quantity, out priceEach)) return;
            extendedPrice = quantity * priceEach;
            if (extendedPrice == 0m) total1.Clear();
            else total1.Text = extendedPrice.ToString("C");
            subTotal += extendedPrice;

            if (ValidateRow(description2, quantity2, price2,
                out quantity, out priceEach)) return;
            extendedPrice = quantity * priceEach;
            if (extendedPrice == 0m) total2.Clear();
            else total2.Text = extendedPrice.ToString("C");
            subTotal += extendedPrice;


            // Updating subtotal textbox
            txtsubTotal.Text = subTotal.ToString("C");


            // Validate the tax rate, if it is true (returns true when error) then return
            if (ValidateTax(taxBox, out taxRate)) return;
            salesTax = subTotal * taxRate;

            // Displaying the taxed prices
            salesTaxTxt.Text = salesTax.ToString();

            // Displaying and calculating the grand total
            grandTotal = subTotal + salesTax;
            grandTotalTxt.Text = grandTotal.ToString("C");


        }

        private bool ValidateRow(TextBox descriptionTxtBox, TextBox quantityTxtBox, 
            TextBox priceTxtBox, out int quantity, out decimal priceEach)
        {
            // Assume the quantity and price values are 0
            quantity = 0;
            priceEach = 0;

            if ((descriptionTxtBox.Text == "") &&
                (quantityTxtBox.Text == "") &&
                (priceTxtBox.Text == ""))
                return false;

            // validating individual textboxes. If empty return true
            if (ValidateIndividualText("Description", descriptionTxtBox)) return true;
            if (ValidateIndividualText("Quantity", quantityTxtBox)) return true;
            if (ValidateIndividualText("Price", priceTxtBox)) return true;

            // All values are present
            // Parse the quantity user input into the quantity variable
            if (!int.TryParse(quantityTxtBox.Text, out quantity))
            {
                DisplayErrorMessage(
                    "Invalid format. Quantity must be an integer", "Invalid value"
                );
                return true;
            }

            // Check if the quantity is between the valid ranges 
            if (quantity < 1 || quantity > 100)
            {
                DisplayErrorMessage(
                    "Invalid quantity: Quantity must be between 1 and 100", "Invalid value"
                );
                return true;
            }

            // Parse the priceEach user input into the priceEach variable
            if (!decimal.TryParse(priceTxtBox.Text, NumberStyles.Currency, null, out priceEach))
            {
                DisplayErrorMessage(
                    "Invalid format. Price must be a decimal", "Invalid value"
                );
                return true;
            }

            // Check if the priceEach is in valid range
            if (priceEach < 0.5m || priceEach > 100000m)
            {
                DisplayErrorMessage(
                    "Price each is out of range", priceTxtBox.Text
                );
                return true;
            }

            // All values have been checked and are all correct
            // Return false to show there is no error
            return false;
        }

        // Checking to see if tax rate is valid. If it is error return true
        private bool ValidateTax(TextBox taxBox, out decimal taxRate)
        {
            // Assume tax is 0 before parsing user input
            taxRate = 0;

            // Check to see if there is a value in the taxbox before evaluating texbox
            if (ValidateIndividualText("Tax", taxBox)) return true;

            // If the value entered by the user contains a % character, parse the value and divide it by 100

            // Changing tax rate to string and removing %
            string taxRateString = taxBox.Text;
            taxRateString = taxRateString.Replace("%", "");

            // Parse the tax rate into tax rate decimal and return true if it is an error (exit statement)
            if (!decimal.TryParse(taxRateString, out taxRate))
            {
                DisplayErrorMessage(
                    "Invalid tax rate value, tax rate must be in decimal form", "Invalid value"
                );
                return true;
            }

            // If tax box contains a % divide by 100, to convert to decimal
            if (taxBox.Text.Contains("%")) taxRate /= 100;


            // Check to see if the tax value is in a valid range
            if (taxRate < 0m || taxRate > 0.2m)
            {
                DisplayErrorMessage(
                    "Tax rate must be between 0 and 0.2", "Invalid value"
                );
                return true;
            }

            // All values have been checked and are correct
            // Return false to show no error
            return false;
        }

        private bool ValidateIndividualText(string name, TextBox txtBox)
        {
            // if textbox has a value return false
            if (txtBox.Text != "")
            { 
                return false;
            }
            DisplayErrorMessage("Missing the " + name + " value." , "Missing value");
            return true;
        }
        

        private void DisplayErrorMessage(string text, string title)
        {
            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
