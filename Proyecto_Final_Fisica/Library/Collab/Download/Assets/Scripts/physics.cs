using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class physics : MonoBehaviour
{
    public GameObject alpha;
    public GameObject electron;
    public GameObject tau;
    public GameObject neutron;
    public GameObject proton;
    public GameObject quark;
    public GameObject positron;
    public GameObject unknow;
    private GameObject clone1;
    private GameObject clone2;
    private GameObject clone3;
    private GameObject pref;

    //Dropdowns
    public GameObject pname1;
    public GameObject pname2;
    public GameObject pname3;

    //Campo para voltaje
    public GameObject voltajeField;

    private double efieldMagnitud;
    private double mfieldMagnitud;
    private double q;
    private double e;
    private double m;
    private double voltaje;
    private double B;

    //Componentes para el calculo de la masa de la particula customizable
    private double unknown_mass;
    private double unknownProton;
    private double unknownNeutron;
    private double unknown_energy;
    public GameObject protonNumberInput;
    public GameObject neutronNumberInput;

    Random rd = new Random(0x6E624EB7u);

    Vector2 vin = new Vector2(0, -1);

    void Start(){
        e = 1.60217646 * (Math.Pow(10, -19));
        B = 10.0e16;
    }

    void Update(){}

    private void FixedUpdate() {
        if (clone1) {
            double eForce = 0;
            if (clone1.transform.position.y > -1.0) {
                eForce = (efieldMagnitud * q);
            }
            //double mForce = -q * (B * clone1.GetComponent<Rigidbody2D>().velocity.magnitude);
            Debug.Log(eForce);
            clone1.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)(eForce + 0), 0), ForceMode2D.Impulse);
        }

        if (clone2) {
            double eForce = 0;
            if (clone2.transform.position.y < -1.0) {
                eForce = (efieldMagnitud * q);
            }
            double mForce = -q * (B * clone2.GetComponent<Rigidbody2D>().velocity.magnitude);
            clone2.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)(eForce+mForce), 0), ForceMode2D.Impulse);
        }

        if (clone3) { }
    }



    public void generate()
    {
        voltaje = double.Parse(voltajeField.GetComponent<Text>().text);
        mfieldMagnitud = voltaje / 0.1;

        string nameTxt1 = pname1.GetComponent<Text>().text;
        string nameTxt2 = pname2.GetComponent<Text>().text;
        string nameTxt3 = pname3.GetComponent<Text>().text;

        Vector2 vin = new Vector2(0, -1);
        int rand_num = rd.NextInt(1, 100);

        clone1 = instantiateParticle(nameTxt1);
        clone1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -rand_num);

        if (!String.Equals(nameTxt2, "None"))
        {
            clone2 = instantiateParticle(nameTxt2);
        }

        if (!String.Equals(nameTxt3, "None"))
        {
            clone3 = instantiateParticle(nameTxt3);
        }

    }

    public double createMass()
    {
        if (protonNumberInput.GetComponent<InputField>().text.Length != 0 || neutronNumberInput.GetComponent<InputField>().text.Length != 0)
        {
            double.TryParse(protonNumberInput.GetComponent<InputField>().text, out unknownProton);
            double.TryParse(neutronNumberInput.GetComponent<InputField>().text, out unknownNeutron);
            unknown_mass = (unknownProton * 1.6727E-24) + (unknownNeutron * 1.6750E-24);
        }
        return unknown_mass;
    }

    public double calculateEnergy()
    {
        if (protonNumberInput.GetComponent<InputField>().text.Length != 0 || neutronNumberInput.GetComponent<InputField>().text.Length != 0)
        {
            double.TryParse(protonNumberInput.GetComponent<InputField>().text, out unknownProton);
            unknown_energy = unknownProton * e;
        }
        return unknown_energy;
    }

    public GameObject instantiateParticle(String nameTxt)
    {
        GameObject prefabParticle;
        switch (nameTxt)
        {
            case "Alpha":
                q = 2*e;
                prefabParticle = alpha;
                m = 6.64466e-27;
                break;
            case "Electron":
                prefabParticle = electron;
                q = -e;
                m = 9.10938e-31;
                break;
            case "Tauon":
                prefabParticle = tau;
                q = -e;
                m = 3.167e-27;
                break;
            case "Neutron":
                prefabParticle = neutron;
                q = 2 * e / 3;
                m = 1.67493e-27;
                break;
            case "Proton":
                prefabParticle = proton;
                q = e;
                m = 1.67262e-27;
                break;
            case "Quark":
                prefabParticle = quark;
                q = -e / 3;
                m = 2.26398e-27;
                break;
            case "Positron":
                prefabParticle = positron;
                q = 2 * e / 3;
                m = 9.10938e-31;
                break;
            default:
                prefabParticle = unknow;
                q = calculateEnergy();
                m = createMass();
                break;
        }
        return Instantiate(prefabParticle, new Vector2(3.27f, 4.47f), Quaternion.identity) as GameObject;
    }
}
