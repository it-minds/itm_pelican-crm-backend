variable "azurermVersion" {
  type        = string
  default     = "3.0.0"
  description = "Input version of azurerm to use or use default 3.0.0"
}

variable "azureLocation" {
  type        = string
  default     = "North Europe"
  description = "Input location of azure group"
}

variable "azurermMsSqlVersion" {
  type        = string
  default     = "12.0"
  description = "Input version of azurerm mssql to use or use default 12.0"
}

variable "resourceGroupName" {
  type        = string
  default     = "Aarhus-Pelican-App"
  description = "Input resource group name to use or use Aarhus-Pelican-App"
}

variable "administrator_login" {
  type = string
}
variable "administrator_login_password" {
  type = string
}
