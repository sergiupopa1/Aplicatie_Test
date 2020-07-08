Feature: Register
	
Scenario: Register new user
	Given I am on the homepage
	And I click on Sign Up button
	When I fill in required data
	Then I get registered

Scenario: Log In
	Given I am on the homepage
	When I click on the login button
	And I enter my credentials
	Then I get logged in

Scenario: Check that Image Slider change the content
	Given I am on the homepage
	When I click on the Previous button from Image Slider	
	Then I see a different product
	When I click on the Next button from Image Slider
	Then I see a different product

Scenario: Buy random phones using given budget
	Given I am logged in
	And I have a budget of 1500$
	When I filter by Phones
	Then I can add to cart 2 random phones that don't exceed my budget

Scenario: Get mean value product cost
	Given I am on the homepage
	When I filter by "<Product>"
	Then I can see in the test output the mean value of each product

	Examples:
		| Product  |
		| Phones   |
		| Laptops  |
		| Monitors |