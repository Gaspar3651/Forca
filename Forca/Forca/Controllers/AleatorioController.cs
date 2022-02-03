﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Windows;
using Forca.Models;

namespace Forca.Controllers
{
    public class AleatorioController : Controller
    {
        private ForcaEntities db = new ForcaEntities();
        SqlConnection conexao = new SqlConnection(@"data source = (local)\SQLEXPRESS; Integrated Security = SSPI; Initial Catalog = Forca;");

        static string Palavra, Dica, PalavraSelecionada;
        static int ContErros = 0;
        static int VerificarResposta;

        // GET: Aleatorio
        public ActionResult Index()
        {
            if (Palavra == null)
            {
                Pular();
            }

            
            // ########## DANDO OS PARABÉNS ##########
            if (Palavra.Equals(PalavraSelecionada))
            {
                VerificarResposta = 1;
                Pular();
            }
            else if(ContErros == 6)
            {
                VerificarResposta = 2;
                Pular();
            }
            else
            {
                VerificarResposta = 0;
            }

            ViewBag.Resposta = Palavra;
            ViewBag.Dica = Dica;
            ViewBag.VerificarResposta = VerificarResposta;
            return View(db.Aleatorio.ToList());
        }



        public ActionResult Chute(string Letra)
        {
            Char[] Alterar = Palavra.ToCharArray();
            string Comparar = Palavra;

            // ########## EXIBINDO A LETRA SE ELA EXISTIR NA PALAVRA ##########
            for (int i = 0; i < PalavraSelecionada.Length; i++)
            {
                string Let = PalavraSelecionada[i].ToString();

                if (Letra.Equals(Let))
                {
                    Alterar[i] = Char.Parse(Let);
                }
                else if ((Let.Equals("Á") || Let.Equals("Â") || Let.Equals("Ã")) && Letra.Equals("A"))
                {
                    Alterar[i] = Char.Parse(Let);
                }
                else if ((Let.Equals("É") || Let.Equals("Ê")) && Letra.Equals("E"))
                {
                    Alterar[i] = Char.Parse(Let);
                }
                else if (Let.Equals("Í") && Letra.Equals("I"))
                {
                    Alterar[i] = Char.Parse(Let);
                }
                else if ((Let.Equals("Ó") || Let.Equals("Ô") || Let.Equals("Õ")) && Letra.Equals("O"))
                {
                    Alterar[i] = Char.Parse(Let);
                }
                else if (Let.Equals("Ú") && Letra.Equals("U"))
                {
                    Alterar[i] = Char.Parse(Let);
                }
                else if (Let.Equals("Ç") && Letra.Equals("C"))
                {
                    Alterar[i] = Char.Parse(Let);
                }
            }
            Palavra = new string(Alterar);

            // ########## VERIFICANDO SE ERROU ##########
            if (Comparar.Equals(Palavra))
            {
                ContErros++;
            }

            
            

            return RedirectToAction("Index");
        }


        public ActionResult Pular()
        {
            Palavra = "";
            conexao.Open();

            string SqlCmd = String.Format("SELECT TOP 1 * " +
                                          "FROM Aleatorio " +
                                          "ORDER BY NEWID()");
            SqlCommand Cmd = new SqlCommand(SqlCmd, conexao);
            SqlDataReader Dados = Cmd.ExecuteReader();

            while (Dados.Read())
            {
                PalavraSelecionada = Dados["Palavra"].ToString();
                Dica = Dados["Dica"].ToString();
            }

            Dados.Close();
            conexao.Close();


            for (int i = 0; i < PalavraSelecionada.Length; i++)
            {
                Palavra += ("_");
            }

            ContErros = 0;

            return RedirectToAction("Index");
        }


        public void Erros()
        {
            string local = "~/Views/Img/Forca-01.png";

            switch (ContErros)
            {
                case 1:
                    local = "~/Views/Img/Forca-02.png";
                    break;
                case 2:
                    local = "~/Views/Img/Forca-03.png";
                    break;
                case 3:
                    local = "~/Views/Img/Forca-04.png";
                    break;
                case 4:
                    local = "~/Views/Img/Forca-05.png";
                    break;
                case 5:
                    local = "~/Views/Img/Forca-06.png";
                    break;
                case 6:
                    local = "~/Views/Img/Forca-07.png";
                    break;

                default:
                    break;
            }

            WebImage wbImage = new WebImage(local);
            wbImage.Resize(350, 386);
            wbImage.FileName = "img.png";
            wbImage.Write();

            
        }

        // GET: Aleatorio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aleatorio aleatorio = db.Aleatorio.Find(id);
            if (aleatorio == null)
            {
                return HttpNotFound();
            }
            return View(aleatorio);
        }

        // GET: Aleatorio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Aleatorio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAleatorio,Palavra,Dica")] Aleatorio aleatorio)
        {
            conexao.Open();
            string SqlCmd = String.Format("INSERT INTO Aleatorio" +
                                          " VALUES ('{0}', '{1}')", 
                                          aleatorio.Palavra.ToUpper(), aleatorio.Dica.ToUpper());
            SqlCommand Cdm = new SqlCommand(SqlCmd, conexao);
            Cdm.ExecuteNonQuery();
            

            conexao.Close();
            return RedirectToAction("Create");
        }

        // GET: Aleatorio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aleatorio aleatorio = db.Aleatorio.Find(id);
            if (aleatorio == null)
            {
                return HttpNotFound();
            }
            return View(aleatorio);
        }

        // POST: Aleatorio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAleatorio,Palavra,Dica")] Aleatorio aleatorio)
        {
            conexao.Open();

            String CmdSql = String.Format("UPDATE Aleatorio " +
                                          "SET Palavra = '{0}', Dica = '{1}'" +
                                          "WHERE IdAleatorio = '{2}'", 
                                          aleatorio.Palavra.ToUpper(), aleatorio.Dica.ToUpper(), aleatorio.IdAleatorio);

            SqlCommand Cmd = new SqlCommand(CmdSql, conexao);
            Cmd.ExecuteNonQuery();

            conexao.Close();
            return RedirectToAction("Index");
        }

        // GET: Aleatorio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aleatorio aleatorio = db.Aleatorio.Find(id);
            if (aleatorio == null)
            {
                return HttpNotFound();
            }
            return View(aleatorio);
        }

        // POST: Aleatorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            conexao.Open();
            string CmdSql = String.Format("DELETE Aleatorio " +
                                          "WHERE IdAleatorio = '{0}'", id);
            SqlCommand Cmd = new SqlCommand(CmdSql, conexao);
            Cmd.ExecuteNonQuery();

            conexao.Close();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        
    }
}
