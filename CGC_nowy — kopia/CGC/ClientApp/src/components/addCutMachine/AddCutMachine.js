
import React, { Component } from "react";
import './AddCutMachine.css'


export class AddCutMachine extends Component {
    displayName = AddCutMachine.name;
    constructor(props) {
        super(props);
        this.state = {
            value: 'laser',
        }
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

    render() {        
        return (
            <div className="addCutMachine">

                <form>

                    <h2>Dodaj maszyne</h2>
                    <h3>Rodzaj maszyny</h3>
                <select onChange={(e) => {
                    this.setState({ value: e.target.value });
                    console.log(this.state)
                }} >
                        <option value="laser">Laser</option>
                        <option value="shears">Nóż</option>
                    </select>
                    <div className="form-group">
                        <button type="reset" className="cancel_machine13" onClick={this.cancel}>Anuluj</button>
                        <button type="submit" className="add_machine1" onClick={this.handleAddCutMachine}>Dodaj</button>
                        
                    </div>

                </form>
            </div>
        );
    }
}