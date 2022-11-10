terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.0.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "Aarhus-Pelican-App"
    storage_account_name = "pelicanazurestorage"
    container_name       = "tfstate-container"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {
    key_vault {
      purge_soft_delete_on_destroy = true
    }
  }
}

data "azurerm_client_config" "current" {}

resource "azurerm_storage_account" "pelican-app-storageaccount" {
  name                     = "pelicanazurestorage"
  resource_group_name      = var.resourceGroupName
  location                 = var.azureLocation
  account_tier             = "Standard"
  access_tier              = "Hot"
  account_replication_type = "LRS"
}

resource "azurerm_mssql_server" "pelican-app-mssqlserver" {
  name                         = "pelican-app-mssqlserver"
  resource_group_name          = var.resourceGroupName
  location                     = var.azureLocation
  version                      = var.azurermMsSqlVersion
  administrator_login          = var.administrator_login
  administrator_login_password = var.administrator_login_password
  tags = {
    environment = "development"
  }
}

resource "azurerm_mssql_database" "pelican-app-mssqldatabase" {
  name      = "pelican-app-mssqldatabase"
  server_id = azurerm_mssql_server.pelican-app-mssqlserver.id
}

resource "azurerm_service_plan" "pelican-appserviceplan" {
  name                = "pelican-appserviceplan"
  location            = var.azureLocation
  resource_group_name = var.resourceGroupName
  os_type             = "Linux"
  sku_name            = "B2"
}
resource "azurerm_linux_web_app" "pelican-linux-web-app" {
  name                = "pelican-linux-web-app"
  resource_group_name = var.resourceGroupName
  location            = var.azureLocation
  service_plan_id     = azurerm_service_plan.pelican-appserviceplan.id
  identity {
    type = "SystemAssigned"
  }

  site_config {}
}

resource "azurerm_static_site" "itm-pelican-crm-frontend" {
  name                = "itm-pelican-crm-frontend"
  resource_group_name = var.resourceGroupName
  location            = "West Europe"
}
