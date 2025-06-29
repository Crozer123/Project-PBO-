﻿using System.Data;
using debugging.PenghubungDB;
using debugging.Model;

namespace debugging
{
    public partial class Chat_admin : Form
    {
        private int? id_customer_aktif = null;
        private string nama_customer = "";
        public Chat_admin()
        {
            InitializeComponent();
            Load_chatUser();
        }

        private void Load_chatUser()
        {
            TxtPesan.Enabled = false;
            BtnSend.Enabled = false;
            LblNamaUser.Text = "";
            flpKolomChat.WrapContents = false;
            flpKolomChat.FlowDirection = FlowDirection.TopDown;
            flpKolomChat.AutoScroll = true;

            flpKolomUser.Controls.Clear();
            using (KoneksiDB chat_db = new KoneksiDB())
            {
                var chat_user = chat_db.data_chat
                    .GroupBy(c => c.id_customer)
                    .Select(g => g.OrderByDescending(w => w.waktu_dikirim).First())
                    .ToList();

                foreach (var user in chat_user)
                {
                    var customer = chat_db.customer.FirstOrDefault(c => c.id_customer == user.id_customer);
                    if (customer == null)
                        continue;

                    string nama = customer.nama;
                    ChatUser chatUser = new ChatUser(nama, user.pesan, user.waktu_dikirim, user.id_customer);
                    chatUser.KlikSemua((s, e) =>
                    {
                        id_customer_aktif = user.id_customer;
                        nama_customer = customer.nama;
                        LblNamaUser.Text = nama_customer;
                        Load_chat(id_customer_aktif.Value);
                        TxtPesan.Enabled = true;
                        BtnSend.Enabled = true;
                    });
                    flpKolomUser.Controls.Add(chatUser);
                }
            }
        }
        private void Load_chat(int id_customer)
        {
            flpKolomChat.Controls.Clear();
            using (KoneksiDB chat_db = new KoneksiDB())
            {
                var Data_chat = chat_db.data_chat
                    .Where(c => c.id_customer == id_customer)
                    .OrderBy(c => c.waktu_dikirim)
                    .ToList();
                foreach (var chat in Data_chat)
                {
                    if (chat == null)
                        continue;
                    bool dariAdmin = chat.admin;
                    string pesan = chat.pesan ?? "";
                    DateTime waktu = chat.waktu_dikirim.ToLocalTime();

                    Gelembung_chat gelembungChat = new Gelembung_chat(pesan, dariAdmin, waktu);
                    gelembungChat.Margin = new Padding(0, 0, 0, 10);
                    //gelembungChat.Dock = DockStyle.Top;
                    gelembungChat.AutoSize = true;
                    gelembungChat.MaximumSize = new Size(flpKolomChat.ClientSize.Width / 2, 0);

                    var panel = new Panel();
                    panel.Width = flpKolomChat.ClientSize.Width;
                    panel.Height = gelembungChat.PreferredSize.Height + 10;
                    panel.Padding = new Padding(0);
                    panel.BackColor = Color.Transparent;

                    panel.Controls.Add(gelembungChat);
                    gelembungChat.Top = 0;
                    if (dariAdmin)
                    {
                        gelembungChat.Left = panel.Width - gelembungChat.PreferredSize.Width - 10;
                        gelembungChat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        //var wrapper = gelembungChat.GetWrappedPanel(flpKolomChat.ClientSize.Width, true);
                    }
                    else
                    {
                        gelembungChat.Left = 10;
                        gelembungChat.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        //var wrapper = gelembungChat.GetWrappedPanel(flpKolomChat.ClientSize.Width, false);
                    }

                    gelembungChat.BringToFront();
                    flpKolomChat.Controls.Add(panel);
                    flpKolomChat.ScrollControlIntoView(panel);
                }
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (id_customer_aktif == null || string.IsNullOrWhiteSpace(TxtPesan.Text)) { return; }
            string isiPesan = TxtPesan.Text.Trim();
            DateTime waktu = DateTime.Now;

            using (KoneksiDB chat = new KoneksiDB())
            {
                var pesan_baru = new Data_chat
                {
                    pesan = isiPesan,
                    waktu_dikirim = DateTime.UtcNow,
                    id_customer = id_customer_aktif.Value,
                    admin = true,
                    foto = null
                };
                chat.data_chat.Add(pesan_baru);
                try
                {
                    chat.SaveChanges();
                }
                catch (Exception ex)
                {
                    string pesanError = ex.Message;
                    if (ex.InnerException != null)
                    {
                        pesanError += "\n\nInner: " + ex.InnerException.Message;
                    }
                    MessageBox.Show("Gagal mengirim pesan: " + pesanError);
                }
            }
            TxtPesan.Clear();
            Load_chat(id_customer_aktif.Value);
        }
        private void flpKolomChat_Paint(object sender, PaintEventArgs e)
        {

        }
        private void TxtPesan_TextChanged(object sender, EventArgs e)
        {

        }
        private void flpKolomUser_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
