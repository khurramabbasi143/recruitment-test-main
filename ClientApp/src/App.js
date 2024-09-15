import React, { useState, useEffect } from 'react';
import axios from 'axios';

const App = () => {
    const [employees, setEmployees] = useState([]);
    const [newEmployee, setNewEmployee] = useState({ name: '', value: 0 });
    const [totalValue, setTotalValue] = useState(null);

    useEffect(() => {
        fetchEmployees();
    }, []);

    const fetchEmployees = async () => {
        const response = await axios.get('/api/list');
        setEmployees(response.data);
    };

    const addEmployee = async () => {
        await axios.post('/api/list', newEmployee);
        fetchEmployees();
        setNewEmployee({ name: '', value: 0 });
    };

    const updateEmployee = async (name) => {
        await axios.put(`/api/list/${name}`, newEmployee);
        fetchEmployees();
    };

    const deleteEmployee = async (name) => {
        await axios.delete(`/api/list/${name}`);
        fetchEmployees();
    };

    const incrementValues = async () => {
        await axios.get('/api/list/increment-values');
        fetchEmployees();
    };

    const getSumOfValues = async () => {
        const response = await axios.get('/api/list/sum-values');
        setTotalValue(response.data);
    };

    return (
        <div>
            <h1>Employee List</h1>
            <ul>
                {employees.map((employee) => (
                    <li key={employee.name}>
                        {employee.name} - {employee.value}
                        <button onClick={() => updateEmployee(employee.name)}>Update</button>
                        <button onClick={() => deleteEmployee(employee.name)}>Delete</button>
                    </li>
                ))}
            </ul>

            <h2>Add New Employee</h2>
            <input
                type="text"
                value={newEmployee.name}
                onChange={(e) => setNewEmployee({ ...newEmployee, name: e.target.value })}
                placeholder="Name"
            />
            <input
                type="number"
                value={newEmployee.value}
                onChange={(e) => setNewEmployee({ ...newEmployee, value: parseInt(e.target.value) })}
                placeholder="Value"
            />
            <button onClick={addEmployee}>Add Employee</button>

            <h2>Actions</h2>
            <button onClick={incrementValues}>Increment Values</button>
            <button onClick={getSumOfValues}>Get Sum of Values</button>

            {totalValue !== null && (
                <div>
                    <h3>Sum of Values:</h3>
                    <p>{totalValue}</p>
                </div>
            )}
        </div>
    );
};

export default App;
