Feature: extraTests
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: 1.Display Home page
	Given I am on the home page
	When I click Home button
	Then the home page is displayed

Scenario: 2.Go to Cart page
	Given I am on the home page
	When I click Cart button
	Then The cart page is displayed

Scenario: 3.Select first product
	Given I am on the home page
	When I click on the first product
	Then display product's price

Scenario: 4.Select first product
	Given I am on the home page
	When I click on the first product
	When I click on Add to Cart button
	When  I click on Cart button
	Then The selected product is displayed with the correct price

#Scenario: 6. Buy a Dell from 2017
#	Given I am on the homepage
#	When I click on Laptops category
#	And I search for a Dell from 2017
#	And I click on Add to Cart button
#	And  I click on Cart button
#	And I place order
#	And I fill the payment required data
#	And I click on Purchase
#	Then I get the order confirmation