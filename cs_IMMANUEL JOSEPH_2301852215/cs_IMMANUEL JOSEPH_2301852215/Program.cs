using cs_IMMANUEL_JOSEPH_2301852215.Model;
using cs_IMMANUEL_JOSEPH_2301852215.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace cs_IMMANUEL_JOSEPH_2301852215
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Menu();
        }

        static void Menu()
        {
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("Supermarket System");
                Console.WriteLine("===================");
                Console.WriteLine("1. Login as User");
                Console.WriteLine("2. Login as Admin");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    choice = -1;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        UserMenu();
                        break;
                    case 2:
                        AdminMenu();
                        break;
                    case 3:
                        Exit();
                        break;
                }

            } while (choice != 3);
        }

        static void UserMenu()
        {
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("User Menu");
                Console.WriteLine("==========");
                Console.WriteLine("1. View Product");
                Console.WriteLine("2. Buy Product");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    choice = -1;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        ViewProduct();
                        break;
                    case 2:
                        BuyProduct();
                        break;
                }

            } while (choice != 3);
        }

        static void AdminMenu()
        {
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("Admin Menu");
                Console.WriteLine("==========");
                Console.WriteLine("1. Insert Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. View Product");
                Console.WriteLine("5. View Transaction");
                Console.WriteLine("6. Exit");
                Console.Write("Choice: ");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    choice = -1;
                }

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        InsertProduct();
                        break;
                    case 2:
                        UpdateProduct();
                        break;
                    case 3:
                        DeleteProduct();
                        break;
                    case 4:
                        ViewProduct();
                        break;
                    case 5:
                        ViewTransaction();
                        break;
                }

            } while (choice != 6);
        }

        static void Exit()
        {
            string greet = "Goodbye! Have a nice day.";
            string name_nim = "Immanuel Joseph - 2301852215";

            foreach(char c in greet)
            {
                Console.Write(c);
                Thread.Sleep(100);
            }

            Console.WriteLine();

            foreach (char n in name_nim)
            {
                Console.Write(n);
                Thread.Sleep(50);
            }

            Console.WriteLine();

            Console.ReadKey();
        }

        static void ViewProduct()
        {
            ProductRepository productRepository = new ProductRepository();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = ".";

            List<ProductModel> productList = productRepository.ViewProducts();

            Console.WriteLine("View Product");
            Console.WriteLine("=============");

            if(productList.Count == 0)
            {
                Console.WriteLine("No Product Available!");
                Console.Write("Press enter to continue...");
                Console.ReadKey();
                return;
            }

            foreach(ProductModel p in productList)
            {
                Console.WriteLine(string.Format("{0,-20} : {1}", "Product ID", p.ProductID));
                Console.WriteLine(string.Format("{0,-20} : {1}", "Product Name", p.ProductName));
                Console.WriteLine(string.Format("{0,-20} : {1}", "Product Quantity", p.ProductQty));
                Console.WriteLine(string.Format("{0,-20} : Rp {1}", "Price", p.ProductPrice.ToString("N0", nfi)));
                Console.WriteLine();
            }
            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();
        }

        static void BuyProduct()
        {
            ProductRepository productRepository = new ProductRepository();
            TransactionRepository transactionRepository = new TransactionRepository();

            int ProductID, ProductQty, ProductPrice, total=0;
            string confirm, PaymentMethod;
            bool done = false;
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = ".";

            List<ProductModel> productList = productRepository.ViewProducts();
            TransactionModel transaction = new TransactionModel();
            List<ProductModel> detailList = new List<ProductModel>();

            Console.WriteLine("Buy Product");
            Console.WriteLine("===========");

            if (productList.Count == 0)
            {
                Console.WriteLine("No Product Available!");
                Console.Write("Press enter to continue...");
                Console.ReadKey();
                return;
            }

            do
            {
                do
                {
                    Console.Write("Input Product ID[1-{0}] : ", productList.Count);
                    try
                    {
                        ProductID = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        ProductID = -1;
                    }
                } while (ProductID < 1 || ProductID > productList.Count);

                do
                {
                    Console.Write("Input Product Quantity[1-{0}] : ", productList.ElementAt(ProductID - 1).ProductQty);
                    try
                    {
                        ProductQty = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        ProductQty = -1;
                    }
                } while (ProductQty < 1 || ProductQty > productList.ElementAt(ProductID - 1).ProductQty);

                productList.ElementAt(ProductID - 1).ProductQty -= ProductQty;
                ProductPrice = productList.ElementAt(ProductID - 1).ProductPrice;

                ProductID = productList.ElementAt(ProductID - 1).ProductID;

                try
                {
                    detailList.Find(x => x.ProductID == ProductID).ProductQty += ProductQty;
                }
                catch (Exception)
                {
                    detailList.Add(new ProductModel(ProductID, "", ProductPrice, ProductQty));
                }

                do
                {
                    Console.Write("Do you want to add another product ? [Yes | No]: ");
                    confirm = Console.ReadLine();
                } while (!confirm.Equals("Yes") && !confirm.Equals("No"));

                done = confirm.Equals("Yes") ? false : true;

                Console.WriteLine();
            } while (!done);

            do
            {
                Console.Write("Choose payment method [Cash | Credit]: ");
                PaymentMethod = Console.ReadLine();
            } while (!PaymentMethod.Equals("Cash") && !PaymentMethod.Equals("Credit"));

            transaction.PaymentMethod = PaymentMethod;
            transaction.detailList = detailList;

            transactionRepository.InsertTransaction(transaction);

            detailList.ForEach(x => total += x.ProductQty * x.ProductPrice);

            Console.WriteLine("Rp {0} Succesfully paid by {1}!", total.ToString("N0", nfi), PaymentMethod);
            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();
        }

        static void InsertProduct()
        {
            ProductRepository productRepository = new ProductRepository();

            string ProductName;
            int ProductPrice, ProductQty;

            Console.WriteLine("Insert Product");
            Console.WriteLine("==============");

            do
            {
                Console.Write("Insert Product Name [Length Between 5-20]: ");
                ProductName = Console.ReadLine();
            } while (ProductName.Length < 5 || ProductName.Length > 20);

            do
            {
                Console.Write("Insert Product Price [1000-1000000]: ");

                try
                {
                    ProductPrice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    ProductPrice = -1;
                }
            } while (ProductPrice < 1000 || ProductPrice > 1000000);

            do
            {
                Console.Write("Insert Product Quantity [1-1000]: ");

                try
                {
                    ProductQty = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    ProductQty = -1;
                }
            } while (ProductQty < 1 || ProductQty > 1000);

            productRepository.InsertProduct(new ProductModel(0, ProductName, ProductPrice, ProductQty));

            Console.WriteLine("The product has been successfully inserted!");
            Console.Write("Press enter to continue...");
            Console.ReadKey();
        }

        static void UpdateProduct()
        {
            ProductRepository productRepository = new ProductRepository();

            int ProductID, ProductPrice, ProductQty;
            string ProductName;

            List<ProductModel> productList = productRepository.ViewProducts();

            Console.WriteLine("Update Product");
            Console.WriteLine("==============");

            if (productList.Count == 0)
            {
                Console.WriteLine("No Product Available!");
                Console.Write("Press enter to continue...");
                Console.ReadKey();
                return;
            }

            do
            {
                Console.Write("Input Product ID[1-{0}] : ", productList.Count);
                try
                {
                    ProductID = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    ProductID = -1;
                }
            } while (ProductID < 1 || ProductID > productList.Count);

            do
            {
                Console.Write("Insert Product Name [Length Between 5-20]: ");
                ProductName = Console.ReadLine();
            } while (ProductName.Length < 5 || ProductName.Length > 20);

            do
            {
                Console.Write("Insert Product Price [1000-1000000]: ");

                try
                {
                    ProductPrice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    ProductPrice = -1;
                }
            } while (ProductPrice < 1000 || ProductPrice > 1000000);

            do
            {
                Console.Write("Insert Product Quantity [1-1000]: ");

                try
                {
                    ProductQty = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    ProductQty = -1;
                }
            } while (ProductQty < 1 || ProductQty > 1000);

            ProductID = productList.ElementAt(ProductID - 1).ProductID;

            productRepository.UpdateProduct(new ProductModel(ProductID, ProductName, ProductPrice, ProductQty));

            Console.WriteLine("The product has been successfully updated!");
            Console.Write("Press enter to continue...");
            Console.ReadKey();
        }

        static void DeleteProduct()
        {
            ProductRepository productRepository = new ProductRepository();

            int ProductID;
            ProductModel product = new ProductModel();
            string confirm;
            bool delete;

            List<ProductModel> productList = productRepository.ViewProducts();

            Console.WriteLine("Delete Product");
            Console.WriteLine("==============");

            if (productList.Count == 0)
            {
                Console.WriteLine("No Product Available!");
                Console.Write("Press enter to continue...");
                Console.ReadKey();
                return;
            }

            do
            {
                Console.Write("Input Product ID[1-{0}] : ", productList.Count);
                try
                {
                    ProductID = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    ProductID = -1;
                }
            } while (ProductID < 1 || ProductID > productList.Count);

            product = productList.ElementAt(ProductID - 1);

            Console.WriteLine();
            Console.WriteLine(string.Format("{0,-20} : {1}" + product.ProductID, "Product ID", product.ProductID));
            Console.WriteLine(string.Format("{0,-20} : {1}", "Product Name", product.ProductName));
            Console.WriteLine(string.Format("{0,-20} : {1}", "Product Quantity", product.ProductQty));
            Console.WriteLine(string.Format("{0,-20} : {1}", "Price", product.ProductPrice));
            Console.WriteLine();

            do
            {
                Console.Write("Are you sure you want to delete this product? [Yes | No]: ");
                confirm = Console.ReadLine();
            } while (!confirm.Equals("Yes") && !confirm.Equals("No"));

            delete = confirm.Equals("Yes") ? true : false;

            if (delete)
            {
                productRepository.DeleteProduct(product.ProductID);
                Console.WriteLine("The product has been successfully deleted!");
            }
            else
            {
                Console.WriteLine("Product deletion cancelled!");
            }

            Console.Write("Press enter to continue...");
            Console.ReadKey();
        }

        static void ViewTransaction()
        {
            TransactionRepository transactionRepository = new TransactionRepository();

            int total;

            List<TransactionModel> transactionList = transactionRepository.ViewTransactions();

            Console.WriteLine("View Transactions");
            Console.WriteLine("=================");

            if (transactionList.Count == 0)
            {
                Console.WriteLine("No Transaction Available!");
                Console.Write("Press enter to continue...");
                Console.ReadKey();
                return;
            }

            foreach (TransactionModel t in transactionList)
            {
                total = 0;
                Console.WriteLine(string.Format("{0,-31}: {1}", "Transaction ID", t.TransactionID));
                Console.WriteLine(string.Format("|{0,-3}| {1,-20} | {2,8} | {3,-7}", "No", "Product Name", "Quantity", "Price"));

                int no = 1;
                foreach(ProductModel p in t.detailList)
                {
                    Console.WriteLine(string.Format("|{0,-3}| {1,-20} | {2,8} | {3,-7}", no++, p.ProductName, p.ProductQty, p.ProductPrice));
                }

                t.detailList.ForEach(x => total += x.ProductQty * x.ProductPrice);

                Console.WriteLine();
                Console.WriteLine(string.Format("{0,-31}: {1} by {2}", "Grand Total", total, t.PaymentMethod));
                Console.WriteLine();
            }

            Console.Write("Press enter to continue...");
            Console.ReadKey();
        }
    }
}
