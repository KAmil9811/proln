import React, { Component } from "react";
import './AddTypeMachine.css';
import Sidebar from '../Sidebar';


export class AddTypeMachine extends Component {
    displayName = AddTypeMachine.name;
    constructor(props) {
        super(props);
    }

    handleAddType = (event) => {
        event.preventDefault();
        const receiver = {
            type: this.refs.type.value,
            user: {
                login: sessionStorage.getItem('login')
            }
        }

        fetch(`api/Machine/Add_Type_Admin`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })

            .then(res => res.json())
            .then(json => {
                
                return (json);
            })
            .then(json => {
                alert("You added new machine type")
                this.props.history.push('/cutmachineedit')
            })
    }

    cancelAddType = (event) => {
        this.props.history.push('/cutmachineedit')
    }

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }


    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to get access</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('machineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (

                <div className="AddTypeMachine">
                    <div>
                        <Sidebar />
                        <div className="title">
                            <h1 className="titletext">Add machine type</h1>
                        </div>
                    </div>
                    <form>

                        <div className="AddTypeMachine_c">


                            <div className="form-group">

                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputType"
                                    placeholder='Enter the machine type'
                                    ref="type"
                                />
                            </div>
                            <div className="add_type_machine_b_c">
                                <button type="button" className="success_add_type_cm " onClick={this.handleAddType}>Add type</button>
                                <button type="button" className="danger_add_type_cm" onClick={this.cancelAddType}>Cancel</button>


                            </div>


                        </div>
                    </form>
                </div>


                );
        }
        else {
            return (
                <div className="HomePage">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
            );
        }
    
    }
}