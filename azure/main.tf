resource "azurerm_resource_group" "simple_group" {
  name     = "MangoServices"
  location = "westeurope"
}

resource "random_string" "rand" {
  length  = 6
  special = false
  upper   = false
}

resource "azurerm_servicebus_namespace" "simple_bus" {
  name                = "mangoweb-${random_string.rand.result}"
  location            = azurerm_resource_group.simple_group.location
  resource_group_name = azurerm_resource_group.simple_group.name
  sku                 = "Standard" # Basic supports only Queues, Standard supports Queues and Topics, 

  tags = {
    environment = "development"
  }
}

resource "azurerm_servicebus_queue" "cart_queue" {
  name         = "EmailShoppingCart"
  namespace_id = azurerm_servicebus_namespace.simple_bus.id

  # enable_partitioning = true
  max_size_in_megabytes = 1024
  max_delivery_count = 5 
  default_message_ttl = "P14D" # 14 days 
  lock_duration = "PT1M" # 1 minute
}

resource "azurerm_servicebus_queue" "registration_queue" {
  name         = "Registration"
  namespace_id = azurerm_servicebus_namespace.simple_bus.id

  # enable_partitioning = true
  max_size_in_megabytes = 1024
  max_delivery_count = 5 
  default_message_ttl = "P14D" # 14 days 
  lock_duration = "PT1M" # 1 minute
}

# resource "azurerm_servicebus_topic" "topic" {
#   name         = "MangoTopic"
#   namespace_id = azurerm_servicebus_namespace.simple_group.id
# }

# resource "azurerm_servicebus_subscription" "sub" {
#   name               = "SimpleSubscription"
#   topic_id           = azurerm_servicebus_topic.topic.id
#   max_delivery_count = 10
# }

# Namespace-Level Policy
resource "azurerm_servicebus_namespace_authorization_rule" "root" {
  name         = "root-policy"
  namespace_id = azurerm_servicebus_namespace.simple_bus.id

  listen = true
  send   = true
  manage = true
}

# Queue-Level Policy
resource "azurerm_servicebus_queue_authorization_rule" "cart_queue_rw" {
  name     = "queue-rw"
  queue_id = azurerm_servicebus_queue.cart_queue.id

  listen = true
  send   = true
}
