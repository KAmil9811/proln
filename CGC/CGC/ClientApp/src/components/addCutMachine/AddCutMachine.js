
import React, { Component } from "react";
import './AddCutMachine.css'
import Sidebar from '../Sidebar';


export class AddCutMachine extends Component {
    displayName = AddCutMachine.name;
    constructor(props) {
        super(props);
        this.state = {
            value: 'laser',
            type: [],
        }
    }


    componentDidMount() {
        
        var table3 = [];
        

        fetch(`api/Machine/Return_All_Type`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
                for (var i = 0; i < json.length; i++) {
                    table3.push({
                        type: json[i],
                    })
                };
                this.setState({
                    type: table3

                });
            })
    }

    handleAddCutMachine = (event) => {
        event.preventDefault();
        const receiver = {
            machines: {
                type: this.state.value
            },
            user: {
                login: sessionStorage.getItem('login')
            }
        }

        fetch(`api/Machine/Add_Machine`, {
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
                const machine2 = json[0].error_Messege
                
                if (machine2 == null) {
                    alert("You add machine")
                    this.props.history.push('/machinewarehouse')
                }
                else {
                    alert("Something went wrong :(")
                }
            })
    }


    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }


    cancel = (event) => {
        this.props.history.push('/machinewarehouse')
    }

    typeSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.type.length; i++) {

            tab.push(< option value={this.state.type[i].type} > {this.state.type[i].type}</option >)


        }
        return (tab)
    }

    render() {   
        let y = this.typeSelector()
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('machineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (<div className="AddCutMachine">

                <Sidebar />
                <div className="title">
                    <h1 className="titletext">Add machine</h1>
                </div>
                <form>
                    <div className="AddCutMachine_c">

                        <div className="AddCutMachine_c_center">

                            <h3 className="h3_add_cut_machine">Select the machine type</h3>
                            <select className="select_add_cut_machine" onChange={(e) => {
                                this.setState({ value: e.target.value });
                                
                            }} >
                                {y}
                            </select>

                        </div>
                        <div className="form-group">

                            <button type="submit" className="success_add_cm" onClick={this.handleAddCutMachine}>Add machine</button>
                            <button type="reset" className="danger_add_cm" onClick={this.cancel}>Cancel</button>



                        </div>


                    </div>
                </form>
            </div>

                );
        }
        else {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
    }
}