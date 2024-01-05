describe('Login Test', () => {
  beforeEach(() => {
    cy.visit('http://localhost:3000/'); 
  });

  it('successfully logs in', () => {

    cy.get('input#email').type('user@example.com');
    cy.get('input#password').type('password123!');

    cy.get('form').submit();
    cy.get('button').contains('Logout'); 
  });

  it('shows an error message for invalid login', () => {

    cy.get('input#email').type('wronguser@example.com');
    cy.get('input#password').type('wrongpassword123!');

    cy.get('form').submit();
    cy.get('.error').should('contain', 'Login failed');
  });
});