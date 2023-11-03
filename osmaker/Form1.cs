using FastColoredTextBoxNS;
using NodeEditor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace osmaker
{
    //man this is just the whole window logic
    public partial class Form1 : Form
    {

        Point offset;
        public static string KernelCode;
        public static string OuterKernelCode;
        string project;
        FContext context = new FContext();
        public static Dictionary<string, string> plugins = new Dictionary<string, string>();
        #region Form inizialization
        public Form1(string project)
        {

            //jsut inits the whole editor

            InitializeComponent();

            nodesControl1.Context = context;
            this.project = project;

            if (File.ReadAllText(project) == "")
            {

                File.WriteAllBytes(project, nodesControl1.Serialize());

            }
            else
            {

                nodesControl1.Deserialize(File.ReadAllBytes(project));

            }

            this.Text = $"{Path.GetFileNameWithoutExtension(project)} - Editor";

        }
        #endregion
        #region ancient stuff
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //shows and acncient about page
            About ab = new About();
            ab.ShowDialog();

        }
        #endregion
        #region code saveing
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //saves, man i should add autosave
            File.WriteAllBytes(project, nodesControl1.Serialize());

        }
        #endregion
        #region Code generation
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //resets the generated kernel code and compiles new one
            KernelCode = "";
            nodesControl1.Execute();
             
            // defines a blank kernel code, adds the generate code to it and writes it into a file
            string basekernel = $"int ch;\r\n\r\n#define WHITE_TXT 0x07\r\n\r\nvoid k_clear_screen();\r\nunsigned int k_printf(char *message, unsigned int line);\r\ntypedef struct __attribute__ ((packed)) {{\r\n    unsigned short di, si, bp, sp, bx, dx, cx, ax;\r\n    unsigned short gs, fs, es, ds, eflags;\r\n}} regs16_t;\r\nextern void int32(unsigned char intnum, regs16_t *regs);\r\nvoid k_memset(char *address, int value, int size) {{\r\n    char *p = address;\r\n    for (int i = 0; i < size; i++) {{\r\n        *p++ = value;\r\n    }}\r\n}}\n{KernelCode};\r\n{OuterKernelCode}\r\nvoid k_clear_screen()\r\n{{\r\n\tchar *vidmem = (char *) 0xb8000;\r\n\tunsigned int i=0;\r\n\twhile(i < (80*25*2))\r\n\t{{\r\n\t\tvidmem[i]=' ';\r\n\t\ti++;\r\n\t\tvidmem[i]=WHITE_TXT;\r\n\t\ti++;\r\n\t}};\r\n}};\r\n\r\nunsigned int k_printf(char *message, unsigned int line)\r\n{{\r\n\tchar *vidmem = (char *) 0xb8000;\r\n\tunsigned int i=0;\r\n\r\n\ti=(line*80*2) + ch;\r\n\r\n\twhile(*message!=0)\r\n\t{{\r\n\t\tif(*message=='\\n')\r\n\t\t{{\r\n\t\t\tline++;\r\n\t\t\ti=(line*80*2);\r\n\t\t\t*message++;\r\n\t\t\tch = 0;\r\n\t\t}} else {{\r\n\t\t\tvidmem[i]=*message;\r\n\t\t\t*message++;\r\n\t\t\ti++;\r\n\t\t\tvidmem[i]=WHITE_TXT;\r\n\t\t\ti++;\r\n\t\t\tch += 2;\r\n\t\t}};\r\n\t}};\r\n\r\n\treturn(1);\r\n}}\r\n\r\nvoid k_drawVline(int X,int Y1,int Y2,int Color)\r\n{{\r\n\r\n\tfor(int y = Y1; y < Y2; y++)\r\n        k_memset((char *)0xA0000 + (y*320+X), Color, 1);\r\n\r\n}}\r\nvoid k_drawPix(int X,int Y,int Color)\r\n{{\r\n\r\n\tk_memset((char *)0xA0000 + (Y*320+X), Color, 1);\r\n\r\n}}\r\n\r\nvoid k_drawRect(int X,int Y,int widht,int height,int color)\r\n{{\r\n\r\n\tfor(int x = X; x < X + widht; x++)\r\n\t{{\r\n\r\n\t\tk_drawVline(x,Y,Y+height,color);\r\n\r\n\t}}\r\n\r\n}}\r\nvoid k_InitVGA(regs16_t regs,int Color)\r\n{{\r\n\tregs.ax = 0x0013;\r\n    int32(0x10, &regs);\r\n\tk_memset((char *)0xA0000, Color, (320*200));\r\n\r\n}}\r\nvoid k_textmode(regs16_t regs)\r\n{{\r\n\r\n    regs.ax = 0x0003;\r\n    int32(0x10, &regs);\r\n\r\n}}\r\nvoid k_readkey(regs16_t regs)\r\n{{\r\n\r\n    regs.ax = 0x0000;\r\n    int32(0x16, &regs);\r\n\r\n}}";
            File.WriteAllText($@"{Directory.GetParent(project).FullName}\Code\kernel.c",basekernel);          
            notifyIcon1.ShowBalloonTip(100);
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            //same as before but now it runs the code
            KernelCode = "";
            nodesControl1.Execute();

            string basekernel = $"int ch;\r\n\r\n#define WHITE_TXT 0x07\r\n\r\nvoid k_clear_screen();\r\nunsigned int k_printf(char *message, unsigned int line);\r\ntypedef struct __attribute__ ((packed)) {{\r\n    unsigned short di, si, bp, sp, bx, dx, cx, ax;\r\n    unsigned short gs, fs, es, ds, eflags;\r\n}} regs16_t;\r\nextern void int32(unsigned char intnum, regs16_t *regs);\r\nvoid k_memset(char *address, int value, int size) {{\r\n    char *p = address;\r\n    for (int i = 0; i < size; i++) {{\r\n        *p++ = value;\r\n    }}\r\n}}\n{KernelCode};\r\n{OuterKernelCode}\r\nvoid k_clear_screen()\r\n{{\r\n\tchar *vidmem = (char *) 0xb8000;\r\n\tunsigned int i=0;\r\n\twhile(i < (80*25*2))\r\n\t{{\r\n\t\tvidmem[i]=' ';\r\n\t\ti++;\r\n\t\tvidmem[i]=WHITE_TXT;\r\n\t\ti++;\r\n\t}};\r\n}};\r\n\r\nunsigned int k_printf(char *message, unsigned int line)\r\n{{\r\n\tchar *vidmem = (char *) 0xb8000;\r\n\tunsigned int i=0;\r\n\r\n\ti=(line*80*2) + ch;\r\n\r\n\twhile(*message!=0)\r\n\t{{\r\n\t\tif(*message=='\\n')\r\n\t\t{{\r\n\t\t\tline++;\r\n\t\t\ti=(line*80*2);\r\n\t\t\t*message++;\r\n\t\t\tch = 0;\r\n\t\t}} else {{\r\n\t\t\tvidmem[i]=*message;\r\n\t\t\t*message++;\r\n\t\t\ti++;\r\n\t\t\tvidmem[i]=WHITE_TXT;\r\n\t\t\ti++;\r\n\t\t\tch += 2;\r\n\t\t}};\r\n\t}};\r\n\r\n\treturn(1);\r\n}}\r\n\r\nvoid k_drawVline(int X,int Y1,int Y2,int Color)\r\n{{\r\n\r\n\tfor(int y = Y1; y < Y2; y++)\r\n        k_memset((char *)0xA0000 + (y*320+X), Color, 1);\r\n\r\n}}\r\nvoid k_drawPix(int X,int Y,int Color)\r\n{{\r\n\r\n\tk_memset((char *)0xA0000 + (Y*320+X), Color, 1);\r\n\r\n}}\r\n\r\nvoid k_drawRect(int X,int Y,int widht,int height,int color)\r\n{{\r\n\r\n\tfor(int x = X; x < X + widht; x++)\r\n\t{{\r\n\r\n\t\tk_drawVline(x,Y,Y+height,color);\r\n\r\n\t}}\r\n\r\n}}\r\nvoid k_InitVGA(regs16_t regs,int Color)\r\n{{\r\n\tregs.ax = 0x0013;\r\n    int32(0x10, &regs);\r\n\tk_memset((char *)0xA0000, Color, (320*200));\r\n\r\n}}\r\nvoid k_textmode(regs16_t regs)\r\n{{\r\n\r\n    regs.ax = 0x0003;\r\n    int32(0x10, &regs);\r\n\r\n}}\r\nvoid k_readkey(regs16_t regs)\r\n{{\r\n\r\n    regs.ax = 0x0000;\r\n    int32(0x16, &regs);\r\n\r\n}}";
            File.WriteAllText($@"{Directory.GetParent(project).FullName}\Code\kernel.c", basekernel);
            notifyIcon1.ShowBalloonTip(100);

            Process terminal = new Process();
            terminal.StartInfo.FileName = "cmd.exe";
            terminal.StartInfo.UseShellExecute = false;
            terminal.StartInfo.RedirectStandardInput = true;
            terminal.Start();
            terminal.StandardInput.AutoFlush = true;
            terminal.StandardInput.WriteLine($@"cd {Directory.GetParent(project).FullName}\Code");
            terminal.StandardInput.WriteLine($@"nasm -f elf32 kernel.asm -o kasm.o");
            terminal.StandardInput.WriteLine($@"i386-elf-gcc\bin\i686-elf-gcc -m32 -c kernel.c -o kc.o");
            terminal.StandardInput.WriteLine($@"i386-elf-gcc\bin\i686-elf-ld -m elf_i386 -T link.ld -o kernel kasm.o kc.o");
            terminal.StandardInput.WriteLine($@"qemu-system-i386 -kernel kernel");
            terminal.Exited += (object Sender, EventArgs E) => { terminal.Dispose(); };

        }
        #endregion
        #region General stuff
        //who the fuck needs this
        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void nodesControl1_OnNodeContextSelected_1(object obj)
        {
            propertyGrid1.SelectedObject = obj;
        }



        //who the fuck added this
        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void nodesControl1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Middle)
            {
                offset.X = MousePosition.X - nodesControl1.Left;
                offset.Y = MousePosition.Y - nodesControl1.Top;
            }

        }

        private void nodesControl1_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Middle)
            {
                nodesControl1.Top = MousePosition.Y - offset.Y;
                nodesControl1.Left = MousePosition.X - offset.X;
            }

        }

        #endregion
    }

    // man this is just the whole editor logic
    public class FContext : INodesContext
    {

        int ln = 0;
        public NodeVisual CurrentProcessingNode { get; set; }
        public event Action<string, NodeVisual, FeedbackType, object, bool> FeedbackInfo;
        public event Action Clear = delegate { };
        #region Plugins


        //you know this manages custom plugins and blocks
        //[Node(name: "Custom Plugin Function", menu: "Plguins",customEditor:typeof(CustomComboBox))]
        //public void AddBlock(List<KeyValuePair<string,object>> args)
        //{

        //    try
        //    {

        //        var a = Regex.Replace(Form1.plugins[(CurrentProcessingNode.CustomEditor as CustomComboBox).Text], "%(INT|BOOL|STRING):(.+?)%", m =>
        //        {

        //            try
        //            {

        //                var b = args.Find(p => p.Key == m.Groups[2].Value);

        //                return b.ToString();
        //            }
        //            catch
        //            {

        //                if (m.Groups[1].Value == "INT")
        //                {
        //                    args.Add(new KeyValuePair<string, object>(m.Groups[2].Value, 0));
        //                }
        //                else if (m.Groups[1].Value == "BOOL")
        //                {
        //                    args.Add(new KeyValuePair<string, object>(m.Groups[2].Value, false));
        //                }
        //                else
        //                {
        //                    args.Add(new KeyValuePair<string, object>(m.Groups[2].Value, ""));
        //                }

        //                return "";
        //            }

        //        });

        //        Form1.KernelCode += a;

        //    }
        //    catch
        //    {

        //    }

        //}
        #endregion
        #region General
        //manages the kernel entry point
        [Node(name: "kernel start", menu: "General", isExecutionInitiator: true, description: "Node from which processing begins. rapresents the following assembly code: jmp $\ntimes 510-($-$$) db 0\ndb 0x55,0xaa")]
        public void KernelStart()
        {
            ln = 0;
            Form1.KernelCode += "\r\n\r\nvoid k_main() \r\n{\r\n\r\n    regs16_t regs;\r\n\t";
        }
        //manages the end of the kernel
        [Node(name: "kernel end", menu: "General", description: "Node from which processing begins. rapresents the following assembly code: jmp $\ntimes 510-($-$$) db 0\ndb 0x55,0xaa")]
        public void KernelE()
        {
            Form1.KernelCode += "\r\n\r\n}";
        }
        #endregion
        #region Inputs
        //prints a line and then returns
        [Node("write a line", "Input", "Basic", "Prints a string to the bios")]
        public void PrintLN(string Text)
        {

            if (Text.StartsWith("@"))
            {
                var a = Text.Remove(0, 1);

                Form1.KernelCode += $"\tk_printf({a},{ln});\nch = 0;\n";
                ln++;

            }
            else
            {

                Form1.KernelCode += $"\tk_printf(\"{Text}\",{ln});\nch = 0;\n";
                ln++;

            }

            Console.WriteLine(Text);

        }
        //same as PrintLN
        [Node("writes", "Input", "Basic", "Prints a string to the bios")]
        public void Print(string Text)
        {

            if (Text.StartsWith("@"))
            {
                var a = Text.Remove(0, 1);

                Form1.KernelCode += $"\tk_printf({Text},{ln});\n";

            }
            else
            {

                Form1.KernelCode += $"\tk_printf(\"{Text}\",{ln});\n";

            }

            Console.WriteLine(Text);

        }
        //reads a character but id does not work
        [Node("read a character", "Input", "Basic", "Reads a character")]
        public void ReadChar(out string Text, string memoryaddress = "char")
        {

            Text = "@"+memoryaddress;

            Form1.KernelCode += $"k_readkey(regs);\n";

        }
        #endregion
        #region Variables and operators
        //math.
        [Node("defines variable", "variables", "int", "sets a variable to a specified character")]
        public void defI(int Value,out string Text, string memoryaddress = "char")
        {

            Form1.KernelCode += $"int {memoryaddress} = {Value};\n";
            Text = "@" + memoryaddress;

        }

        [Node("if equals (int)", "variables", "int", "sets a variable to a specified character")]
        public void ife(int Value1, int Value2, string truememoryaddress = "char", string falsememoryaddress = "char")
        {

            Form1.KernelCode += $"if ({Value1} == {Value2}) {{{truememoryaddress}();}} else {{{falsememoryaddress}();}}\n";

        }

        [Node("set variable (int)", "variables", "int", "sets a variable to a specified character")]
        public void setI(int Value, out string Text, string memoryaddress = "char")
        {
            Form1.KernelCode += $"{memoryaddress} = {Value};\n";
            Text = "@" + memoryaddress;

        }

        [Node("get variable (int)", "variables", "int", "sets a variable to a specified character")]
        public void getI(out string Text, string memoryaddress = "char")
        {
           
            Text = "@" + memoryaddress;

        }

        [Node("add to variable (int)", "operations", "int", "sets a variable to a specified character")]
        public void addI(int value1, string memoryaddress = "char")
        {

            Form1.KernelCode += $"{memoryaddress} += {value1};\n";

        }

        [Node("subtract to variable (int)", "operations", "int", "sets a variable to a specified character")]
        public void subI(int value1, string memoryaddress = "char")
        {

            Form1.KernelCode += $"{memoryaddress} -= {value1};\n";

        }

        [Node("set variable to additon(int)", "operations", "int", "sets a variable to a specified character")]
        public void additI(int value1, int value2, string memoryaddress = "char")
        {

            Form1.KernelCode += $"{memoryaddress} = {value1} + {value2};\n";

        }

        [Node("set variable to subtraction(int)", "operations", "int", "sets a variable to a specified character")]
        public void subtrI(int value1, int value2, string memoryaddress = "char")
        {

            Form1.KernelCode += $"{memoryaddress} = {value1} - {value2};\n";

        }
        #endregion
        #region basics and Graphics
        [Node("define While", "functions", "Basic", "sets a variable to a specified character")]
        public void Wdef(string contition)
        {
            Form1.KernelCode += $"while ({contition}) \n{{\n";

        }

        [Node("End While", "functions", "Basic", "sets a variable to a specified character")]
        public void wstop()
        {

            Form1.KernelCode += $"\n}}\n";

        }

        [Node("define function", "functions", "Basic", "sets a variable to a specified character")]
        public void FDef(out string Text, string memoryaddress = "char")
        {
            Form1.KernelCode += $"void {memoryaddress}() \n{{\n";
            Text = memoryaddress;
        }

        [Node("End Function", "functions", "Basic", "sets a variable to a specified character")]
        public void FDefStop()
        {

            Form1.KernelCode += $"\n}}\n";

        }

        [Node("Call Funtion", "functions", "Basic", "sets a variable to a specified character")]
        public void FCall(string memoryaddress = "char")
        {

            Form1.KernelCode += $"{memoryaddress}();\n";

        }

        [Node("Inizialize canvas", "Graphics", "Basic", "sets a variable to a specified character")]
        public void Canvinit(string color)
        {

            Form1.KernelCode += $"k_InitVGA(regs,{color});\n";

        }

        [Node("draw pixels on canvas", "Graphics", "Basic", "sets a variable to a specified character")]
        public void Canvdrawpix(int X,int Y,string color)
        {

            Form1.KernelCode += $"k_drawPix({X},{Y},{color});\n";

        }

        [Node("draw rectange on canvas", "Graphics", "Basic", "sets a variable to a specified character")]
        public void Canvdrawrect(int X, int Y, int Widht, int Height, string color)
        {

            Form1.KernelCode += $"k_drawRect({X},{Y},{Widht},{Height},{color});\n";

        }

        [Node("draw vertical line on canvas", "Graphics", "Basic", "sets a variable to a specified character")]
        public void Canvdrawline(int X1, int Y1, int Y2, string color)
        {

            Form1.KernelCode += $"k_drawVline({X1},{Y1},{Y2},{color});\n";

        }
        [Node("get predefined screen widht", "Graphics", "Basic", "sets a variable to a specified character",false)]
        public void gpw(out int value)
        {

            value = 320;

        }
        [Node("get predefined screen height", "Graphics", "Basic", "sets a variable to a specified character", false)]
        public void gph(out int value)
        {

            value = 200;

        }
        [Node("Black", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void bl(out string value)
        {

            value = "0000";

        }
        [Node("blue", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void blue(out string value)
        {

            value = "0001";

        }
        [Node("Green", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void gre(out string value)
        {

            value = "02";

        }
        [Node("Cyan", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void cy(out string value)
        {

            value = "3";

        }
        [Node("Red", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void red(out string value)
        {

            value = "4";

        }
        [Node("Magenta", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void mag(out string value)
        {

            value = "5";

        }
        [Node("Brown", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Brown(out string value)
        {

            value = "6";

        }
        [Node("Light Gray", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Light_Gray(out string value)
        {

            value = "7";

        }
        [Node("Dark Gray", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Dark_Gray(out string value)
        {

            value = "8";

        }
        [Node("Light Blue", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Light_Blue(out string value)
        {

            value = "9";

        }

        [Node("Light Green", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Light_Green(out string value)
        {

            value = "10";

        }
        [Node("Light Cyan", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Light_Cyan(out string value)
        {

            value = "11";

        }
        [Node("Light Red", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Light_Red(out string value)
        {

            value = "12";

        }
        [Node("Light Magenta", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Light_Magenta(out string value)
        {

            value = "13";

        }
        [Node("Yellow", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void Yellow(out string value)
        {

            value = "14";

        }
        [Node("Light Blue", "Colors", "Basic", "sets a variable to a specified character", false)]
        public void White(out string value)
        {

            value = "15";

        }
        #endregion
        #region Advanceds

        [Node(name: "C script", menu: "Advanceds", customEditor: typeof(ScriptBOX),width:600,height:400)]
        public void Cscript()
        {

            try
            {

                Form1.KernelCode += (CurrentProcessingNode.CustomEditor as ScriptBOX).Text;

            }
            catch
            {

            }

        }

        #endregion
    }

}
