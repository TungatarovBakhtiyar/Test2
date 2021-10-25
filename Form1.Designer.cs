namespace Cryptography
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.CaesarKey = new System.Windows.Forms.TextBox();
            this.VigenerKey = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SKL = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.NKL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choose encryption:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // CaesarKey
            // 
            this.CaesarKey.Location = new System.Drawing.Point(164, 72);
            this.CaesarKey.Name = "CaesarKey";
            this.CaesarKey.Size = new System.Drawing.Size(128, 22);
            this.CaesarKey.TabIndex = 3;
            this.CaesarKey.Visible = false;
            // 
            // VigenerKey
            // 
            this.VigenerKey.Location = new System.Drawing.Point(164, 37);
            this.VigenerKey.Name = "VigenerKey";
            this.VigenerKey.Size = new System.Drawing.Size(128, 22);
            this.VigenerKey.TabIndex = 4;
            this.VigenerKey.Visible = false;
            this.VigenerKey.TextChanged += new System.EventHandler(this.VigenerKey_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(17, 124);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(522, 120);
            this.textBox3.TabIndex = 5;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // SKL
            // 
            this.SKL.AutoSize = true;
            this.SKL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SKL.Location = new System.Drawing.Point(25, 39);
            this.SKL.Name = "SKL";
            this.SKL.Size = new System.Drawing.Size(101, 20);
            this.SKL.TabIndex = 6;
            this.SKL.Text = "Enter string:";
            this.SKL.Visible = false;
            this.SKL.Click += new System.EventHandler(this.SKL_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(199, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Enter message:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(199, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Encrypted message:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(199, 433);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(164, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Decrypted message:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(16, 270);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(522, 160);
            this.textBox4.TabIndex = 10;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(17, 464);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(523, 138);
            this.textBox5.TabIndex = 11;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(560, 171);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 34);
            this.button3.TabIndex = 12;
            this.button3.Text = "Encrypt";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Encrypt);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(560, 327);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 34);
            this.button4.TabIndex = 13;
            this.button4.Text = "Decrypt";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Decrypt);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(298, 624);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 38);
            this.button5.TabIndex = 14;
            this.button5.Text = "Exit";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Exit);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Binary",
            "Caesar",
            "Caesar with key",
            "Vigener",
            "Table",
            "Table with key",
            "Magic",
            "Polibian",
            "Playfair",
            "Trisemus",
            "Gronsfeld",
            "Witston",
            "XOR",
            "Gamma",
            "Elgamal",
            "DES",
            "AES",
            "RSA",
            "Diffi-Hellman",
            "GOST(nr)"});
            this.comboBox1.Location = new System.Drawing.Point(226, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.Text = "Binary";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.SelectionChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(298, 61);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 33);
            this.button6.TabIndex = 16;
            this.button6.Text = "Clear";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Clr);
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(406, 13);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(212, 96);
            this.log.TabIndex = 17;
            this.log.Text = "";
            this.log.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(560, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 18;
            this.button1.Text = "Log";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.show_hide);
            // 
            // NKL
            // 
            this.NKL.AutoSize = true;
            this.NKL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NKL.Location = new System.Drawing.Point(25, 72);
            this.NKL.Name = "NKL";
            this.NKL.Size = new System.Drawing.Size(115, 20);
            this.NKL.TabIndex = 19;
            this.NKL.Text = "Enter number:";
            this.NKL.Click += new System.EventHandler(this.NKL_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 674);
            this.Controls.Add(this.NKL);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.log);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SKL);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.VigenerKey);
            this.Controls.Add(this.CaesarKey);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Ciphers";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CaesarKey;
        private System.Windows.Forms.TextBox VigenerKey;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label SKL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.RichTextBox log;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label NKL;
    }
}

