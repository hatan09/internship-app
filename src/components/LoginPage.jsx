import React from 'react';
import {Link, useNavigate } from "react-router-dom";
import pic from '../images/login_img.jpg';
import {login} from '../api/Api'

function LoginPage() {
    let navigate = useNavigate();

    const handleLogin = async (event) => {
        // Prevent page reload
        event.preventDefault();

        var { username, pass } = document.forms[0];

        let result = await login(username.value, pass.value);
        const role = result.roles[0];

        switch(role){
            case 'string' : localStorage.setItem("userId", result.id); console.log(result.id); localStorage.setItem("token", result.accessToken); console.log(result.accessToken); navigate("/student", { replace: true }); break;
            case 'instructor': {
                localStorage.setItem("userId", result.id); 
                console.log(result.id); 
                localStorage.setItem("token", result.accessToken); 
                console.log(result.accessToken); 
                navigate("/instructor", { replace: true }); break;
            }
            case 'student' : {
                localStorage.setItem("userId", result.id); 
                console.log(result.id); 
                localStorage.setItem("token", result.accessToken); 
                console.log(result.accessToken); 
                navigate("/student", { replace: true }); break;
            }
            case 'recruiter' : {
                localStorage.setItem("userId", result.id); 
                console.log(result.id); 
                localStorage.setItem("token", result.accessToken); 
                console.log(result.accessToken); 
                navigate("/recruiter", { replace: true }); break;
            }
            default : console.log('not found role'); break;
        }
      };

    return ( 
        <div className="login-page">
            <div className="login-panel">
                <div className="login-banner">
                    <span className="banner-title">Welcome to Internship App</span>
                </div>
                <div className="login-area">
                    <div className="login-pic"> <img src={pic} alt="identity" /></div>
                    <form className='login-form' onSubmit={handleLogin} method="POST">
                        <span name="login-title">Login</span>
                        <ul>
                            <li>
                                <input type="text" name="username" id="" placeholder='Username'/>
                            </li>
                            <li>
                                <input type="password" name="pass" id="" placeholder='Password'/>
                            </li>
                            <li>
                                <button type="submit">Login</button>
                            </li>
                        </ul>

                        <div className="forget">
                            <span>Forgot your account?</span>
                        </div>
                        <div className="create-acc">
                            <Link to='/register'> Create your account </Link>
                        </div>
                    </form>
                </div>
                

                
            </div>

        </div>
     );
}

export default LoginPage;