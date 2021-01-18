
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
                console.log(json);
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
                'Content-Type': 'application/json'
            }
        })

            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json);
            })
            .then(json => {
                const machine2 = json[0].error_Messege
                console.log(machine2)
                if (machine2 == null) {
                    console.log("Dodano maszynę")
                    this.props.history.push('/machinewarehouse')
                }
                else {
                    alert("Coś poszło nie tak :(")
                }
            })
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
        return (
            <div className="AddCutMachine">

                    <Sidebar />  
                    <div className="addCutMachine">
                  
                        <form>

                            <h2>Dodaj maszyne</h2>
                            <h3>Rodzaj maszyny</h3>
                        <select onChange={(e) => {
                            this.setState({ value: e.target.value });
                            console.log(this.state)
                        }} >
                                {y}
                            </select>
                           <div className="form-group">
                          
                                <button type="reset" className="danger_add_cm" onClick={this.cancel}>Anuluj</button>
                              
                                <button type="submit" className="success_add_cm" onClick={this.handleAddCutMachine}>Dodaj</button>
                               
                            <div className="nextline"> </div>
                            </div>

                        </form>
                    </div>
            </div>

               );
    }
}