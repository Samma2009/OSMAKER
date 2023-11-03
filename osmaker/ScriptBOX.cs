using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osmaker
{
    public partial class ScriptBOX : FastColoredTextBox
    {

        FastColoredTextBoxNS.AutocompleteMenu menu;

        public ScriptBOX()
        {
            InitializeComponent();

            menu = new AutocompleteMenu(this);
            menu.MinFragmentLength = 1;
            menu.BackColor = Color.FromArgb(30,30,30);
            menu.HoveredColor = Color.FromArgb(42, 86, 155);
            menu.SelectedColor = Color.FromArgb(42, 86, 155);
            menu.ForeColor = Color.White;
            string words = "auto break case char const continue default do double else enum extern float for goto if int long register return short signed sizeof static struct switch typedef union unsigned void volatile while k_printf(\"\",0) ch regs k_InitVGA(regs,1) k_clear_screen() k_drawVline(0,0,0,1) k_drawPix(0,0,1) k_drawRect(0,0,10,10,1) k_textmode(regs) k_readkey(regs) ";
            menu.Items.SetAutocompleteItems(words.Split(' '));
        }

        private void ScriptBOX_Load(object sender, EventArgs e)
        {

        }
    }
}
