Feature: extraTests

Scenario: 1.Display Home page
	Given I am on the home page
	When I click Home button
	Then the home page is displayed

Scenario: 2.Go to Cart page
	Given I am on the home page
	When I go to cart
	Then The cart page is displayed

Scenario: 3.Select first product
	Given I am on the home page
	When I select the first product
	Then display product's price

Scenario: 4.Select first product and check cart
	Given I am on the home page
	When I select the first product
	Then I add product to cart
	When I go to cart
	Then The selected product is displayed with the correct price

Scenario: 5.Check all pages from the header
	Given I am on the home page
	When I click on <Page>
	Then I can see the correct <Page>

	Examples: 
	| Page     |
	| Home     |
	| Contact  |
	| About us |
	| Cart     |
	| Log in   |
	| Sign up  |

Scenario: 6. Buy a Dell from 2017
	Given I am on the home page
	When I filter by Laptops
	When I search for 2017 Dell
	Then I add product to cart
	When I place the order	
	Then I get the order confirmation

Scenario: 7. Buy an Apple monitor
	Given I am on the home page
	When I filter by Monitors
	And I search for Apple monitor
	Then I add product to cart
	When I place the order	
	Then I get the order confirmation
	Then The cart is empty

Scenario: 8. Buy products within budget
	Given I am on the home page
	And I have a budget of 1500$
	And I select a phone, a laptop and a monitor within budget
	When I place the order	
	Then I get the order confirmation
