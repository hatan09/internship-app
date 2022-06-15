import React, { useState, useEffect } from 'react';
import {reg, getAllGroup} from '../api/Api'
import DepartmentItem from './DepartmentItem';


function RegisterPage() {
    const [selected, setSelected] = useState(0);
    const [group, setGroup] = useState([]);

    const loadData = async () => {
        var groups = await getAllGroup();
        setGroup(groups);
    }

    useEffect(() => {
        loadData();
    }, []);

    const handleRegister = async (event) => {
        // Prevent page reload
        event.preventDefault();

        if(selected === 0) {
            console.log('fail');
            return;
        }
        console.log('pass');
        var { name, bd, username, pass, studentId, email, credit, gpa, phone } = document.forms[0];

        let result = await reg({
            fullName: name.value,
            birthdate: bd.value.toString('dd/MM/yyyy'),
            username: username.value, 
            password: pass.value, 
            email: email.value, 
            studentId: studentId.value, 
            credit: credit.value, 
            gpa: gpa.value,
            departmentId : selected,
            phoneNumber: phone.value});
            
      };

    return ( 
        <div className="register-page">
            <div className="register-panel">
                <div className="register-banner">
                    <span className="banner-title">Welcome to Internship App</span>
                </div>
                <div className="register-area">
                    <div className="department-list">
                        <ul className='list'>
                            {group.map(item => (
                                <li key={item.id}><DepartmentItem item={item} onClick={setSelected} disabled={(item.id === selected)}/></li>
                            ))}
                        </ul>
                    </div>
                    <form className='register-form' onSubmit={handleRegister} method='POST'>
                        <span name="register-title">Register</span>
                        <ul>
                            <li>
                                <input type="text" name="name" id="" placeholder='Your Full Name'/>
                            </li>
                            <li>
                                <input type="date" name="bd" id=""/>
                            </li>
                            <li>
                                <input type="text" name="username" id="" placeholder='Username'/>
                            </li>
                            <li>
                                <input type="password" name="pass" id="" placeholder='Password'/>
                            </li>
                            <li>
                                <input type="text" name="studentId" id="" placeholder='Student ID'/>
                            </li>
                            <li>
                                <input type="text" name="email" id="" placeholder='Email Address'/>
                            </li>
                            <li>
                                <input type="text" name="credit" id="" placeholder='Credit Achieved'/>
                            </li>
                            <li>
                                <input type="text" name="gpa" id="" placeholder='GPA'/>
                            </li>
                            <li>
                                <input type="text" name="phone" id="" placeholder='Phone Number'/>
                            </li>
                            <li>
                                <button type="submit">Proceed</button>
                            </li>
                        </ul>
                    </form>
                </div>
                

                
            </div>

        </div>
     );
}

export default RegisterPage;