import { Calculator } from './calculator';
import { expect } from 'chai';
import 'mocha';

describe('Calculator', () => {
    it('should add numbers', () => {
        const calc = new Calculator();
        const result = calc.add(10, 20);

        expect(result).to.equal(30);
    });

    it('should multiply numbers', () => {
        const calc = new Calculator();
        const result = calc.multiply(10, 20);

        expect(result).to.equal(200);
    });
});