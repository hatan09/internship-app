import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import {userContext} from './userContext'

import "./App.css";

import InstructorPage from "./components/Instructor/InstructorPage";
import StudentPage from "./components/Student/StudentPage";
import RecuruiterPage from './components/Recruiter/RecruiterPage';
import LoginPage from "./components/LoginPage";
import WelcomePage from "./components/WelcomePage";
import StudentJobPage from "./components/Student/StudentJobPage/StudentJobPage";
import ProfilePage from "./components/Student/ProfilePage/ProfilePage";
import RegisterManagePage from "./components/Instructor/RegisterManagePage/RegisterManagePage";
import StudentManagePage from "./components/Instructor/StudentManagePage/StudentManagePage"
import ViewJobPage from "./components/Instructor/ViewJobPage/ViewJobPage";
import RegisterPage from './components/RegisterPage'
import AddJobPage from './components/Recruiter/AddJobPage/AddJobPage'
import StudentInfoPage from './components/Recruiter/StudentInfoPage/StudentInfoPage'

function App() {
    return (
        <userContext.Provider value={{}}>
            <Router>
                <div className="App">
                    <Routes>
                        <Route path="/login" element={<LoginPage/>} />
                        <Route path="/register" element={<RegisterPage/>} />
                        <Route path="/student" element={<StudentPage/>} >
                            <Route index element={<WelcomePage title="Welcome Students to Internship App"/>} />
                            <Route path="jobs" element={<StudentJobPage/>} />
                            <Route path="profile" element={<ProfilePage/>} />
                        </Route>
                        <Route path="/instructor" element={<InstructorPage/>} >
                            <Route index element={<WelcomePage title="Welcome Teachers to Internship App"/>} />
                            <Route path="registers" element={<RegisterManagePage/>} />
                            <Route path="students" element={<StudentManagePage/>} />
                            <Route path="jobs" element={<ViewJobPage/>} />
                        </Route>
                        <Route path="/recruiter" element={<RecuruiterPage/>} >
                            <Route index element={<WelcomePage title="Welcome Recruiters to Internship App"/>} />
                            <Route path="jobs" element={<AddJobPage/>} />
                            <Route path="students" element={<StudentInfoPage/>} />
                        </Route>
                    </Routes>
                </div>
            </Router>
        </userContext.Provider>
        
     );
}

export default App;