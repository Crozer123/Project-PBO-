﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using debugging.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace debugging.Service
{
    public class Cetak_Struk
    {
        public static void BuatStrukPenyewaan(
            string namaCustomer,
            string namaProduk,
            int jumlah,
            decimal totalDp,
            DateTime tanggalSewa,
            DateTime tanggalKembali,
            string metodePembayaran,
            string noRekening,
            string alamat,
            string filePath = "struk_penyewaan.pdf")
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Struk Penyewaan";
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12);

            int y = 40;
            gfx.DrawString("Struk Penyewaan", font, XBrushes.Black, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
            y += 40;
            gfx.DrawString($"Tanggal Sewa   : {tanggalSewa:dd/MM/yyyy}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Tanggal Kembali: {tanggalKembali:dd/MM/yyyy}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Nama           : {namaCustomer}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Alamat         : {alamat}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Produk         : {namaProduk}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Jumlah         : {jumlah}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Total DP       : Rp {totalDp:N0}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Metode         : {metodePembayaran}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"No Rek         : {noRekening}", font, XBrushes.Black, 40, y);

            document.Save(filePath);
        }
        public static void BuatStrukPembelian(
            string namaCustomer,
            string namaProduk,
            int jumlah,
            decimal totalHarga,
            DateTime tanggal,
            string metodePembayaran,
            string noRekening,
            string filePath = "struk_pembelian.pdf")
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Struk Pembelian";
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12);

            int y = 40;
            gfx.DrawString("Struk Pembelian", font, XBrushes.Black, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
            y += 40;
            gfx.DrawString($"Tanggal        : {tanggal:dd/MM/yyyy}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Nama           : {namaCustomer}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Produk         : {namaProduk}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Jumlah         : {jumlah}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Total Harga    : Rp {totalHarga:N0}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"Metode         : {metodePembayaran}", font, XBrushes.Black, 40, y); y += 25;
            gfx.DrawString($"No Rek         : {noRekening}", font, XBrushes.Black, 40, y);

            document.Save(filePath);
        }
        public static void BuatStrukPembelianKeranjang(
           string namaUser,
           List<ProdukDiStruk> produkList,
           decimal total,
           DateTime tanggal,
           string metodePembayaran,
           string noRekening)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Struk Pembelian Keranjang";
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12);

            int y = 40;
            gfx.DrawString("STRUK PEMBELIAN KERANJANG", new XFont("Verdana", 16), XBrushes.Black, 40, y);
            y += 30;
            gfx.DrawString($"Nama: {namaUser}", font, XBrushes.Black, 40, y);
            y += 20;
            gfx.DrawString($"Tanggal: {tanggal:dd/MM/yyyy HH:mm}", font, XBrushes.Black, 40, y);
            y += 20;
            gfx.DrawString($"Metode Pembayaran: {metodePembayaran}", font, XBrushes.Black, 40, y);
            y += 20;
            gfx.DrawString($"No Rekening: {noRekening}", font, XBrushes.Black, 40, y);
            y += 30;

            gfx.DrawString("Daftar Produk:", font, XBrushes.Black, 40, y);
            y += 20;
            gfx.DrawString("Nama         Jumlah     Harga Satuan     Subtotal", font, XBrushes.Black, 40, y);
            y += 20;

            foreach (var item in produkList)
            {
                string nama = item.NamaProduk;
                int jumlah = item.Jumlah;
                decimal harga = item.HargaProduk;
                decimal subtotal = harga * jumlah;
                gfx.DrawString($"{nama,-12} {jumlah,6}   Rp {harga,10:N0}   Rp {subtotal,12:N0}", font, XBrushes.Black, 40, y);
                y += 20;
            }

            y += 10;
            gfx.DrawString($"Total: Rp {total:N0}", new XFont("Verdana", 12), XBrushes.Black, 40, y);

            string filePath = Path.Combine(Environment.CurrentDirectory, "struk_pemmbelian.pdf");
            document.Save(filePath);
            document.Close();
        }
    }
}
