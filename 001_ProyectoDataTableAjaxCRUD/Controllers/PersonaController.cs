using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Xml.Linq;
using _001_ProyectoDataTableAjaxCRUD.Models;

namespace _001_ProyectoDataTableAjaxCRUD.Controllers
{
    public class PersonaController : Controller
    {
        // GET: Persona
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarProductos()
        {
            using (PROYECT_CRUD_ASPEntities3 db = new PROYECT_CRUD_ASPEntities3())
            {
                var productosConCategorias = db.tb_producto.Include("tb_categoria").Where(producto => producto.activo == true).ToList();

                var listaProductos = productosConCategorias.Select(producto => new
                {
                    producto.cod_producto,
                    producto.descrip_prod,
                    producto.stock,
                    producto.pventa,
                    producto.fecha_venc,
                    producto.activo,
                    Categoria = new
                    {
                        producto.tb_categoria.cod_categoria,
                        producto.tb_categoria.categoria_prod
                    }
                }).ToList();

                return Json(new { data = listaProductos }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult obtenerProducto(int idProducto)
        {
            using (PROYECT_CRUD_ASPEntities3 db = new PROYECT_CRUD_ASPEntities3())
            {
                tb_producto producto = db.tb_producto.Include("tb_categoria").FirstOrDefault(p => p.cod_producto == idProducto);

                if (producto != null)
                {
                    var productoConCategoria = new
                    {
                        producto.cod_producto,
                        producto.descrip_prod,
                        producto.stock,
                        producto.pventa,
                        producto.fecha_venc,
                        producto.activo,
                        Categoria = new
                        {
                            producto.tb_categoria.cod_categoria,
                            producto.tb_categoria.categoria_prod
                        }
                    };

                    return Json(productoConCategoria, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "Producto no encontrado" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult ListarCategorias()
        {
            using (PROYECT_CRUD_ASPEntities3 db = new PROYECT_CRUD_ASPEntities3())
            {
                var categorias = db.tb_categoria.ToList();


                var listadoCategoria = categorias.Select(categoria => new
                {
                    categoria.cod_categoria,
                    categoria.categoria_prod
                }).ToList();

                return Json(new { listadoCategoria }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AgregarProducto(tb_producto producto)
        {
            using(PROYECT_CRUD_ASPEntities3 db = new PROYECT_CRUD_ASPEntities3())
            {
                if(ModelState.IsValid)
                {
                    producto.activo = true;
                    db.tb_producto.Add(producto);
                    db.SaveChanges();

                    return Json(new { data = true, message = "El Producto fue agregado exitosamente" });
                }
                else
                {
                    return Json(new { data = true, message = "Hubo un error" });
                }
            }
        }

        [HttpPost]
        public JsonResult EditarProducto(tb_producto producto)
        {
            using(PROYECT_CRUD_ASPEntities3 db = new PROYECT_CRUD_ASPEntities3())
            {
                if (ModelState.IsValid)
                {
                    var productoExistente = db.tb_producto.Find(producto.cod_producto);
                    if(productoExistente != null)
                    {
                        productoExistente.descrip_prod = producto.descrip_prod;
                        productoExistente.pventa = producto.pventa;
                        productoExistente.cod_categoria = producto.cod_categoria;
                        productoExistente.stock = producto.stock;
                        productoExistente.fecha_venc = producto.fecha_venc;

                        db.SaveChanges();

                        return Json(new { success = true, message = "El Producto fue actualizado exitosamente" });

                    }
                    else
                    {
                        return Json(new { success = false, message = "Producto no encontrado" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Hubo un error en los datos ingresados" });
                }
            }
        }

        public JsonResult DesactivarProducto(int cod_Producto)
        {
            using(PROYECT_CRUD_ASPEntities3 db = new PROYECT_CRUD_ASPEntities3())
            {
                tb_producto producto = db.tb_producto.Find(cod_Producto);
                
                if(producto != null)
                {
                    producto.activo = true;
                    db.SaveChanges();
                    return Json(new { success = true, message = "El Producto fue desactivado exitosamente" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Producto no encontrado" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GenerarPDF()
        {
            IEnumerable<tb_producto> productosConCategorias = new List<tb_producto>();

            using (PROYECT_CRUD_ASPEntities3 db = new PROYECT_CRUD_ASPEntities3())
            {
                productosConCategorias = db.tb_producto.Include("tb_categoria").Where(producto => producto.activo == true).ToList();

                var listaProductos = productosConCategorias.Select(producto => new
                {
                    producto.cod_producto,
                    producto.descrip_prod,
                    producto.stock,
                    producto.pventa,
                    producto.fecha_venc,
                    producto.activo,
                    Categoria = new
                    {
                        producto.tb_categoria.cod_categoria,
                        producto.tb_categoria.categoria_prod
                    }
                }).ToList();
            }

            var data = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);

                    page.Header().ShowOnce().Row(row =>
                    {
                        row.ConstantItem(140).Height(60).Placeholder();


                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Codigo Estudiante SAC").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("Jr. Las mercedes N378 - Lima").FontSize(9);
                            col.Item().AlignCenter().Text("987 987 123 / 02 213232").FontSize(9);
                            col.Item().AlignCenter().Text("codigo@example.com").FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272")
                            .AlignCenter().Text("RUC 21312312312");

                            col.Item().Background("#257272").Border(1)
                            .BorderColor("#257272").AlignCenter()
                            .Text("Boleta de venta").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("B0001 - 234");


                        });
                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().Column(col2 =>
                        {
                            col2.Item().Text("Datos del cliente").Underline().Bold();

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Nombre: ").SemiBold().FontSize(10);
                                txt.Span("Mario mendoza").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("DNI: ").SemiBold().FontSize(10);
                                txt.Span("978978979").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Direccion: ").SemiBold().FontSize(10);
                                txt.Span("av. miraflores 123").FontSize(10);
                            });
                        });

                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn(2);

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("ID").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Categoria").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Nombre").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Stock").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Fecha de Vencimiento").FontColor("#fff");
                            });

                            foreach (var item in productosConCategorias)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.cod_producto.ToString()).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.tb_categoria.categoria_prod).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.descrip_prod).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.stock.ToString()).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.pventa.ToString()).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2).Text(item.fecha_venc.ToString()).FontSize(10);
                            }

                        });
                    });


                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(data);
            return File(stream, "application/pdf", "detalleventa.pdf");
        }
    }
}